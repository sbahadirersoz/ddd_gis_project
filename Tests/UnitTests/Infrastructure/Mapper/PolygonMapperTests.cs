using FluentAssertions;

using gis.Domain.Aggregates;
using gis.Domain.Entities.Coord;
using gis.Domain.Entities.Coord.Polygon;
using gis.Domain.Entities.IDs;
using gis.Domain.Entities.Information.Polygon;
using gis.Domain.Entities.WKT;

using Gis.Infrastructure.Mapper;

namespace Tests.UnitTests.Infrastructure.Mapper;

// Contract: PolygonAggregate.Reconstitute(id, shell, holes, name, description, status) rebuilds an
// aggregate from stored data; PolygonMapper turns it into a PolygonEntity and back without losing anything.
public class PolygonMapperTests
{
    // closed rings: first point == last point
    private static readonly (double lon, double lat)[] ShellRing =
        [(29.00, 41.00), (29.10, 41.00), (29.10, 41.10), (29.00, 41.10), (29.00, 41.00)];

    private static readonly (double lon, double lat)[] HoleRing =
        [(29.02, 41.02), (29.04, 41.02), (29.04, 41.04), (29.02, 41.04), (29.02, 41.02)];

    private const string ShellWkt =
        "POLYGON ((29 41, 29.1 41, 29.1 41.1, 29 41.1, 29 41))";

    private static List<CoordinateValueObject> Ring((double lon, double lat)[] points) =>
        points.Select(p => CoordinateValueObject.ForPolygonCoordCreation(
            Latitude.Create(p.lat).Value, Longitude.Create(p.lon).Value).Value).ToList();

    private static (double, double)[] LonLats(IEnumerable<CoordinateValueObject> coords) =>
        coords.Select(c => (c.Longitude.Value, c.Latitude.Value)).ToArray();

    private static PolygonAggregate MakePolygon(string? desc, PolygonStatus status, params (double, double)[][] holes)
    {
        var shell = PolygonShell.Create(Ring(ShellRing), WellKnownText.Create(ShellWkt).Value).Value;
        return PolygonAggregate.Reconstitute(
            PolygonID.New(),
            shell,
            holes.Select(Ring).ToList(),
            PolygonName.CreateFromString("park").Value,
            PolygonDescription.CreateFromString(desc).Value,
            status);
    }

    [Fact]
    public void RoundTrip_KeepsEveryField()
    {
        var original = MakePolygon("green area", PolygonStatus.INACTIVE, HoleRing);

        var back = PolygonMapper.ToDomain(PolygonMapper.ToDbEntity(original));

        back.Id.Should().Be(original.Id);
        back.Name.Value.Should().Be("park");
        back.Description.Value.Should().Be("green area");
        back.Status.Should().Be(PolygonStatus.INACTIVE);
        back.Shell.wkt.Value.Should().Be(ShellWkt);
        LonLats(back.Shell.Coordinates).Should().Equal(ShellRing);
        back.Holes.Should().HaveCount(1);
        LonLats(back.Holes[0]).Should().Equal(HoleRing);
    }

    [Fact]
    public void ToDbEntity_StoresHolesAsInteriorRings()
    {
        var entity = PolygonMapper.ToDbEntity(MakePolygon("d", PolygonStatus.ACTIVE, HoleRing));

        entity.Polygon.NumInteriorRings.Should().Be(1);
        entity.Polygon.Shell.Coordinates.Select(c => (c.X, c.Y)).Should().Equal(ShellRing);
        entity.Polygon.Holes[0].Coordinates.Select(c => (c.X, c.Y)).Should().Equal(HoleRing);
    }

    [Fact]
    public void RoundTrip_WithoutHoles_HasNoHoles()
    {
        var back = PolygonMapper.ToDomain(PolygonMapper.ToDbEntity(MakePolygon("d", PolygonStatus.ACTIVE)));

        back.Holes.Should().BeEmpty();
    }

    [Fact]
    public void ToDbEntity_EmptyDescription_IsEmptyStringNotNull()
    {
        // Desc column is NOT NULL
        var entity = PolygonMapper.ToDbEntity(MakePolygon(null, PolygonStatus.ACTIVE));

        entity.Desc.Should().Be(string.Empty);
    }
}
