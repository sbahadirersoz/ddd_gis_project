using FluentAssertions;

using Gis.Application.Dtos;
using Gis.Application.Features.Polygon.Queries.FindPolygonById;

using gis.Domain.Aggregates;
using gis.Domain.Entities.Coord;
using gis.Domain.Entities.Coord.Polygon;
using gis.Domain.Entities.IDs;
using gis.Domain.Entities.Information.Polygon;
using gis.Domain.Entities.WKT;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern.Errors;

using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace Tests.UnitTests.Application.Features.Polygon.Queries;

public class FindPolygonByIdHandlerTests
{
    private readonly IPolygonRepository _repo = Substitute.For<IPolygonRepository>();
    private readonly FindPolygonByIdHandler _handler;

    public FindPolygonByIdHandlerTests()
    {
        _handler = new FindPolygonByIdHandler(_repo);
    }

    private static readonly (double lon, double lat)[] ShellRing =
        [(29.00, 41.00), (29.10, 41.00), (29.10, 41.10), (29.00, 41.10), (29.00, 41.00)];

    private static readonly (double lon, double lat)[] HoleRing =
        [(29.02, 41.02), (29.04, 41.02), (29.04, 41.04), (29.02, 41.04), (29.02, 41.02)];

    private const string ShellWkt = "POLYGON ((29 41, 29.1 41, 29.1 41.1, 29 41.1, 29 41))";

    private static List<CoordinateValueObject> Ring((double lon, double lat)[] points) =>
        points.Select(p => CoordinateValueObject.ForPolygonCoordCreation(
            Latitude.Create(p.lat).Value, Longitude.Create(p.lon).Value).Value).ToList();

    private static PolygonAggregate MakePolygon(params (double, double)[][] holes) =>
        PolygonAggregate.Reconstitute(
            PolygonID.New(),
            PolygonShell.Create(Ring(ShellRing), WellKnownText.Create(ShellWkt).Value).Value,
            holes.Select(Ring).ToList(),
            PolygonName.CreateFromString("park").Value,
            PolygonDescription.CreateFromString("green area").Value,
            PolygonStatus.ACTIVE);

    // the repository only answers for this exact id, so the test also checks the id is passed through
    private void RepoReturns(PolygonAggregate agg) =>
        _repo.FindByIdAsync(Arg.Is<PolygonID>(x => x.Value == agg.Id.Value), Arg.Any<bool>(), Arg.Any<CancellationToken>())
            .Returns(agg);

    [Fact]
    public async Task Existing_ReturnsPolygon()
    {
        var agg = MakePolygon();
        RepoReturns(agg);

        var result = await _handler.Handle(new FindPolygonByIdQuery(agg.Id.Value));

        result.IsSuccess.Should().BeTrue();
        result.Value!.id.Should().Be(agg.Id.Value);
        result.Value.name.Should().Be("park");
        result.Value.desc.Should().Be("green area");
        result.Value.status.Should().Be(PolygonStatus.ACTIVE);
        result.Value.wkt.Should().Be(ShellWkt);
        result.Value.holes.Should().BeEmpty();
    }

    [Fact]
    public async Task Missing_ReturnsEntityNotFound()
    {
        _repo.FindByIdAsync(Arg.Any<PolygonID>(), Arg.Any<bool>(), Arg.Any<CancellationToken>()).ReturnsNull();

        var result = await _handler.Handle(new FindPolygonByIdQuery(Guid.NewGuid()));

        result.Error.Should().Be(RepositoryErrors.ENTITY_NOT_FOUND);
    }

    [Fact]
    public async Task WithHole_ReturnsHoleCoordinates()
    {
        var agg = MakePolygon(HoleRing);
        RepoReturns(agg);

        var result = await _handler.Handle(new FindPolygonByIdQuery(agg.Id.Value));

        result.Value!.holes.Should().HaveCount(1);
        result.Value.holes[0].Should().Equal(HoleRing.Select(p => new CoordinateDto(p.lon, p.lat)));
    }

    [Fact]
    public async Task EmptyId_ReturnsEmptyIdFormat_AndDoesNotHitRepository()
    {

        var result = await _handler.Handle(new FindPolygonByIdQuery(Guid.Empty));
        result.Error.Should().Be(DomainErrors.EMPTY_ID_FORMAT);
         await _repo.DidNotReceive().FindByIdAsync(Arg.Any<PolygonID>(), Arg.Any<bool>(), Arg.Any<CancellationToken>());

    }

}
