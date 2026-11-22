using FluentAssertions;

using gis.Domain.Entities.Coord;
using gis.Domain.Entities.Coord.Polygon;
using gis.Domain.Entities.WKT;
using gis.Domain.ResultPattern.Errors;

using Gis.Infrastructure.Topology_Services;

namespace Tests.UnitTests.Infrastructure.TopologyServices;

public class TopologySuitePolygonValidationContractImplTests
{
    private readonly TopologySuitePolygonValidationContractImpl _contract = new();

    // shell: 29.00-29.10 lon, 41.00-41.10 lat
    private static readonly PolygonShell Shell = PolygonShell.Create(
        Ring([(29.00, 41.00), (29.10, 41.00), (29.10, 41.10), (29.00, 41.10), (29.00, 41.00)]),
        WellKnownText.Create("POLYGON ((29 41, 29.1 41, 29.1 41.1, 29 41.1, 29 41))").Value).Value;

    private static List<CoordinateValueObject> Ring((double lon, double lat)[] points) =>
        points.Select(p => CoordinateValueObject.ForPolygonCoordCreation(
            Latitude.Create(p.lat).Value, Longitude.Create(p.lon).Value).Value).ToList();

    [Fact]
    public void HOLE_INSIDE_SHELL_RETURNS_SUCCESS()
    {
        var hole = Ring([(29.02, 41.02), (29.04, 41.02), (29.04, 41.04), (29.02, 41.04), (29.02, 41.02)]);

        var result = _contract.IsHoleInsideTheShell(hole, Shell);

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void HOLE_OUTSIDE_SHELL_RETURNS_HOLE_NOT_INSIDE_SHELL()
    {
        var hole = Ring([(30.02, 42.02), (30.04, 42.02), (30.04, 42.04), (30.02, 42.04), (30.02, 42.02)]);

        var result = _contract.IsHoleInsideTheShell(hole, Shell);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be(BusinessRuleErrors.PolygonRules.HOLE_NOT_INSIDE_SHELL.Code);
    }

    [Fact]
    public void HOLE_CROSSING_SHELL_RETURNS_HOLE_NOT_INSIDE_SHELL()
    {
        // half inside, half outside the shell's east edge (29.10)
        var hole = Ring([(29.08, 41.02), (29.12, 41.02), (29.12, 41.04), (29.08, 41.04), (29.08, 41.02)]);

        var result = _contract.IsHoleInsideTheShell(hole, Shell);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("BusinessRules.Polygon.HoleNotInsideShell");
    }
}
