using FluentAssertions;

using Gis.Application.Common;
using Gis.Application.Dtos;
using Gis.Application.Features.Polygon.Commands.AddHoleToPolygon;

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

// Contract: POST /api/polygons/{id}/holes -> loads the polygon, adds the hole through the domain service
// (which checks hole-inside-shell), persists with Update + SaveChanges, returns id and all holes.
public class AddHoleToPolygonCommandHandlerTests
{
    private readonly IPolygonRepository _repo = Substitute.For<IPolygonRepository>();
    private readonly IUnitOfWork _uow = Substitute.For<IUnitOfWork>();
    private readonly AddHoleToPolygonCommandHandler _handler;

    public AddHoleToPolygonCommandHandlerTests()
    {
        var wkt = new TopologySuiteWktContractImpl();
        var rules = new PolygonBusinessRulesFactory(new TopologySuitePolygonValidationContractImpl());
        var service = new PolygonDomainServiceContract(_repo, wkt, rules);
        _handler = new AddHoleToPolygonCommandHandler(service, _uow, _repo);
    }

    private static readonly (double lon, double lat)[] ShellRing =
        [(29.00, 41.00), (29.10, 41.00), (29.10, 41.10), (29.00, 41.10), (29.00, 41.00)];

    private static readonly (double lon, double lat)[] InsideHole =
        [(29.02, 41.02), (29.04, 41.02), (29.04, 41.04), (29.02, 41.04), (29.02, 41.02)];

    private static readonly (double lon, double lat)[] OutsideHole =
        [(30.02, 42.02), (30.04, 42.02), (30.04, 42.04), (30.02, 42.04), (30.02, 42.02)];

    private static List<CoordinateValueObject> Ring((double lon, double lat)[] points) =>
        points.Select(p => CoordinateValueObject.ForPolygonCoordCreation(
            Latitude.Create(p.lat).Value, Longitude.Create(p.lon).Value).Value).ToList();

    private static PolygonAggregate MakePolygon() =>
        PolygonAggregate.Reconstitute(
            PolygonID.New(),
            PolygonShell.Create(Ring(ShellRing),
                WellKnownText.Create("POLYGON ((29 41, 29.1 41, 29.1 41.1, 29 41.1, 29 41))").Value).Value,
            [],
            PolygonName.CreateFromString("park").Value,
            PolygonDescription.CreateFromString("green area").Value,
            PolygonStatus.ACTIVE);

    private static AddHoleToPolygonCommand Command(Guid id, (double lon, double lat)[] hole) =>
        new(id, hole.Select(p => new CoordinateDto(p.lon, p.lat)).ToList());

    private PolygonAggregate RepoHasPolygon()
    {
        var agg = MakePolygon();
        _repo.FindByIdAsync(Arg.Is<PolygonID>(x => x.Value == agg.Id.Value), Arg.Any<bool>(), Arg.Any<CancellationToken>())
            .Returns(agg);
        return agg;
    }

    private async Task NothingSaved()
    {
        _repo.DidNotReceive().Update(Arg.Any<PolygonAggregate>());
        await _uow.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HoleInsideShell_IsSaved_AndReturnsHoles()
    {
        var agg = RepoHasPolygon();

        var result = await _handler.Handle(Command(agg.Id.Value, InsideHole), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value!.id.Should().Be(agg.Id.Value);
        result.Value.holes.Should().HaveCount(1);
        result.Value.holes[0].Should().Equal(InsideHole.Select(p => new CoordinateDto(p.lon, p.lat)));
        _repo.Received(1).Update(Arg.Is<PolygonAggregate>(a => a.Id.Value == agg.Id.Value && a.Holes.Count == 1));
        await _uow.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HoleOutsideShell_ReturnsHoleNotInsideShell_AndSavesNothing()
    {
        var agg = RepoHasPolygon();

        var result = await _handler.Handle(Command(agg.Id.Value, OutsideHole), CancellationToken.None);

        result.Error.Should().Be(BusinessRuleErrors.PolygonRules.HOLE_NOT_INSIDE_SHELL);
        await NothingSaved();
    }

    [Fact]
    public async Task MissingPolygon_ReturnsEntityNotFound_AndSavesNothing()
    {
        _repo.FindByIdAsync(Arg.Any<PolygonID>(), Arg.Any<bool>(), Arg.Any<CancellationToken>()).ReturnsNull();

        var result = await _handler.Handle(Command(Guid.NewGuid(), InsideHole), CancellationToken.None);

        result.Error.Should().Be(RepositoryErrors.ENTITY_NOT_FOUND);
        await NothingSaved();
    }

    [Fact]
    public async Task EmptyId_ReturnsEmptyIdFormat_AndDoesNotHitRepository()
    {
        var result = await _handler.Handle(Command(Guid.Empty, InsideHole), CancellationToken.None);

        result.Error.Should().Be(DomainErrors.EMPTY_ID_FORMAT);
        await _repo.DidNotReceive().FindByIdAsync(Arg.Any<PolygonID>(), Arg.Any<bool>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task InvalidCoordinate_ReturnsFailure_AndSavesNothing()
    {
        var agg = RepoHasPolygon();
        (double, double)[] badHole = [(29.02, 95), (29.04, 41.02), (29.04, 41.04), (29.02, 95)];

        var result = await _handler.Handle(Command(agg.Id.Value, badHole), CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        await NothingSaved();
    }
}
