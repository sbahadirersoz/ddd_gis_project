using FluentAssertions;

using Gis.Application.Common;
using Gis.Application.Features.Polygon.Commands.UpdatePolygon;

using gis.Domain.Aggregates;
using gis.Domain.BusinessRules.PolygonRules;
using gis.Domain.Entities.Coord;
using gis.Domain.Entities.Coord.Polygon;
using gis.Domain.Entities.IDs;
using gis.Domain.Entities.Information.Polygon;
using gis.Domain.Entities.WKT;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern.Errors;
using gis.Domain.Services;

using Gis.Infrastructure.Topology_Services;

using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace Tests.UnitTests.Application.Features.Polygon;

// Contract: PATCH /api/v1/polygons/{id} changes name, desc and status; a null field is left as is.
// Only changed fields are checked (renaming to its own name must not be "already exists").
// Persists with Update + SaveChanges, returns id, name, desc, status.
public class UpdatePolygonCommandHandlerTests
{
    private readonly IPolygonRepository _repo = Substitute.For<IPolygonRepository>();
    private readonly IUnitOfWork _uow = Substitute.For<IUnitOfWork>();
    private readonly UpdatePolygonCommandHandler _handler;

    public UpdatePolygonCommandHandlerTests()
    {
        var wkt = new TopologySuiteWktContractImpl();
        var rules = new PolygonBusinessRulesFactory(new TopologySuitePolygonValidationContractImpl());
        var service = new PolygonDomainServiceContract(_repo, wkt, rules);
        _handler = new UpdatePolygonCommandHandler(service, _uow, _repo);
        _repo.IsPolygonNameExistsAsync(Arg.Any<PolygonName>(), Arg.Any<CancellationToken>()).Returns(false);
    }

    private static readonly (double lon, double lat)[] ShellRing =
        [(29.00, 41.00), (29.10, 41.00), (29.10, 41.10), (29.00, 41.10), (29.00, 41.00)];

    private static List<CoordinateValueObject> Ring((double lon, double lat)[] points) =>
        points.Select(p => CoordinateValueObject.ForPolygonCoordCreation(
            Latitude.Create(p.lat).Value, Longitude.Create(p.lon).Value).Value).ToList();

    // stored polygon: name "park", desc "green area", ACTIVE
    private PolygonAggregate RepoHasPolygon()
    {
        var agg = PolygonAggregate.Reconstitute(
            PolygonID.New(),
            PolygonShell.Create(Ring(ShellRing),
                WellKnownText.Create("POLYGON ((29 41, 29.1 41, 29.1 41.1, 29 41.1, 29 41))").Value).Value,
            [],
            PolygonName.CreateFromString("park").Value,
            PolygonDescription.CreateFromString("green area").Value,
            PolygonStatus.ACTIVE);
        _repo.FindByIdAsync(Arg.Is<PolygonID>(x => x.Value == agg.Id.Value), Arg.Any<bool>(), Arg.Any<CancellationToken>())
            .Returns(agg);
        return agg;
    }

    private async Task SavedOnce(Guid id)
    {
        _repo.Received(1).Update(Arg.Is<PolygonAggregate>(a => a.Id.Value == id));
        await _uow.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    private async Task NothingSaved()
    {
        _repo.DidNotReceive().Update(Arg.Any<PolygonAggregate>());
        await _uow.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task NewName_IsSaved_OtherFieldsKept()
    {
        var agg = RepoHasPolygon();

        var result = await _handler.Handle(new UpdatePolygonCommand(agg.Id.Value, name: "lake"), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(new UpdatePolygonCommandResponse(agg.Id.Value, "lake", "green area", PolygonStatus.ACTIVE));
        await SavedOnce(agg.Id.Value);
    }

    [Fact]
    public async Task NewDescription_IsSaved()
    {
        var agg = RepoHasPolygon();

        var result = await _handler.Handle(new UpdatePolygonCommand(agg.Id.Value, desc: "city park"), CancellationToken.None);

        result.Value.Should().Be(new UpdatePolygonCommandResponse(agg.Id.Value, "park", "city park", PolygonStatus.ACTIVE));
        await SavedOnce(agg.Id.Value);
    }

    [Fact]
    public async Task NewStatus_IsSaved()
    {
        var agg = RepoHasPolygon();

        var result = await _handler.Handle(new UpdatePolygonCommand(agg.Id.Value, status: PolygonStatus.INACTIVE), CancellationToken.None);

        result.Value!.status.Should().Be(PolygonStatus.INACTIVE);
        await SavedOnce(agg.Id.Value);
    }

    [Fact]
    public async Task OwnNameWithNewDescription_IsNotAlreadyExist()
    {
        // the DB finds "park" because it is this polygon's own name
        var agg = RepoHasPolygon();
        _repo.IsPolygonNameExistsAsync(Arg.Any<PolygonName>(), Arg.Any<CancellationToken>()).Returns(true);

        var result = await _handler.Handle(new UpdatePolygonCommand(agg.Id.Value, name: "park", desc: "city park"), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value!.desc.Should().Be("city park");
        await SavedOnce(agg.Id.Value);
    }

    [Fact]
    public async Task NameOfAnotherPolygon_ReturnsAlreadyExist_AndSavesNothing()
    {
        var agg = RepoHasPolygon();
        _repo.IsPolygonNameExistsAsync(Arg.Is<PolygonName>(n => n.Value == "lake"), Arg.Any<CancellationToken>()).Returns(true);

        var result = await _handler.Handle(new UpdatePolygonCommand(agg.Id.Value, name: "lake"), CancellationToken.None);

        result.Error.Should().Be(RepositoryErrors.VALUE_ALREADY_EXIST_IN_DB);
        await NothingSaved();
    }

    [Fact]
    public async Task SameValues_ReturnsSameCredentials_AndSavesNothing()
    {
        var agg = RepoHasPolygon();

        var result = await _handler.Handle(
            new UpdatePolygonCommand(agg.Id.Value, "park", "green area", PolygonStatus.ACTIVE), CancellationToken.None);

        result.Error.Should().Be(DomainServiceErrors.SAME_CREDENTIALS_FOR_UPDATING);
        await NothingSaved();
    }

    [Fact]
    public async Task NoFields_ReturnsSameCredentials_AndSavesNothing()
    {
        var agg = RepoHasPolygon();

        var result = await _handler.Handle(new UpdatePolygonCommand(agg.Id.Value), CancellationToken.None);

        result.Error.Should().Be(DomainServiceErrors.SAME_CREDENTIALS_FOR_UPDATING);
        await NothingSaved();
    }

    [Fact]
    public async Task MissingPolygon_ReturnsEntityNotFound_AndSavesNothing()
    {
        _repo.FindByIdAsync(Arg.Any<PolygonID>(), Arg.Any<bool>(), Arg.Any<CancellationToken>()).ReturnsNull();

        var result = await _handler.Handle(new UpdatePolygonCommand(Guid.NewGuid(), name: "lake"), CancellationToken.None);

        result.Error.Should().Be(RepositoryErrors.ENTITY_NOT_FOUND);
        await NothingSaved();
    }

    [Fact]
    public async Task EmptyId_ReturnsEmptyIdFormat_AndDoesNotHitRepository()
    {
        var result = await _handler.Handle(new UpdatePolygonCommand(Guid.Empty, name: "lake"), CancellationToken.None);

        result.Error.Should().Be(DomainErrors.EMPTY_ID_FORMAT);
        await _repo.DidNotReceive().FindByIdAsync(Arg.Any<PolygonID>(), Arg.Any<bool>(), Arg.Any<CancellationToken>());
    }
}
