using FluentAssertions;

using gis.Domain.Aggregates;
using gis.Domain.Entities.Coord;
using gis.Domain.Entities.IDs;
using gis.Domain.Entities.Information;
using gis.Domain.Entities.WKT;

using Gis.Infrastructure.Entity;

using Xunit.Abstractions;

namespace Tests.UnitTests.Domain.Services.PoiTests;

public class PoiEntityTest
{
    private readonly ITestOutputHelper _testOutputHelper;

    private static POIAggregate CreatePoi(double  longitude  ,double latitude, string name)
    {
        var id = PointID.New();
        var lat = Latitude.Create(latitude).Value;
        var lon = Longitude.Create(longitude).Value;
        var wkt = WellKnownText.Create($"POINT ({lon.Value} {lat.Value})").Value;
        var coordinate = CoordinateValueObject.FromLonLat(lat, lon, wkt).Value;
        return POIAggregate.Reconstitute(
            id,
            coordinate,
            PointDescription.FromString("Sultanahmet").Value,
            PointName.FromString(name).Value,
            POIStatus.ACTIVE);
    }
    
    [Fact]
    public void Create_POI_AND_CHECK_X_Y_AXESES()
    {
        POIAggregate unit = CreatePoi(100.04, 42.55, "MockPointName");
        PoiEntity entity = PoiEntity.AggToEntity(unit);
        unit.CoordinateValueObject.Latitude.Value.Equals(entity.Point.Y).Should().Be(true);
        unit.CoordinateValueObject.Longitude.Value.Equals(entity.Point.X).Should().Be(true);
    }
    
}