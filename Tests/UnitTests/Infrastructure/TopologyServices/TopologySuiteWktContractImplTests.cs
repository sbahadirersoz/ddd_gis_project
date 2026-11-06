using FluentAssertions;

using gis.Domain.ResultPattern.Errors;

using Gis.Infrastructure.Topology_Services;

namespace Tests.UnitTests.Infrastructure.TopologyServices;

public class TopologySuiteWktContractImplTests
{
    private readonly TopologySuiteWktContractImpl _contract = new();

    [Theory]
    [InlineData("POINT(45 22)", "POINT(45 22)")]
    [InlineData("point (45 22)", "POINT (45 22)")]
    [InlineData("POINT (-179.5 -89.5)", "POINT (-179.5 -89.5)")]
    [InlineData("POINT(100 45)", "POINT(100 45)")]
    public void PARSE_POINT_WKT_VALID_POINT_RETURNS_WKT(string wkt, string expected)
    {
        var result = _contract.ParsePointWkt(wkt);

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be(expected);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("HELLO")]
    [InlineData("POINT")]
    [InlineData("POINT {45,22}")]
    [InlineData("POINT(45)")]
    [InlineData("POINT(45 22")]
    [InlineData("POINT(abc def)")]
    [InlineData("POINT EMPTY")]
    [InlineData("POINT(NaN NaN)")]
    [InlineData("LINESTRING (0 0, 1 1)")]
    [InlineData("POLYGON ((0 0, 1 0, 1 1, 0 0))")]
    public void PARSE_POINT_WKT_INVALID_FORMAT_RETURNS_FAILURE(string? wkt)
    {
        var result = _contract.ParsePointWkt(wkt!);

        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public void PARSE_POINT_WKT_LONGITUDE_OUT_OF_RANGE_RETURNS_FAILURE()
    {
        var result = _contract.ParsePointWkt("POINT(200 20)");

        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public void PARSE_POINT_WKT_READS_SECOND_VALUE_AS_LATITUDE()
    {
        var result = _contract.ParsePointWkt("POINT(20 95)");

        result.Error.Should().Be(DomainErrors.POIErrors.Coordinate.LATITUDE_COORDINATE_IS_OVER_MAX_VALUE_ERROR);
    }
}
