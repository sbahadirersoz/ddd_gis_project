using System.Text.Json;
using gis.ApplicationLayer.Mapper.LineMapper;
using gis.InfrastructureLayer.HelperMethods;
using Xunit.Abstractions;

namespace Tests.UnitTests.Common;

public class CommonGeometryHelperTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public CommonGeometryHelperTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public Task FromCoordinateVOListToLineStringWKTTest()
    {
        var list = new List<(double x, double y)>
        {
            (29.0601, 40.9902),
            (28.9784, 41.0082),
            (29.0122, 41.0428)
        };


        var primitiveToCoordinates = LineAggregateVOMapper.PrimitiveListToCoordinatesList(list).Value;
        var str = CommonGeometryHelpers.FromCoordinateVOListToLineStringWKT(primitiveToCoordinates);
         _testOutputHelper.WriteLine(JsonSerializer.Serialize(str));
         return Task.CompletedTask;
    }
}