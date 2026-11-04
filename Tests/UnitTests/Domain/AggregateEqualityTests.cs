using FluentAssertions;

using gis.Domain.Aggregates;
using gis.Domain.Entities.Coord;
using gis.Domain.Entities.IDs;
using gis.Domain.Entities.Information;
using gis.Domain.Entities.WKT;

namespace Tests.UnitTests.Domain;

public class AggregateEqualityTests
{
    [Fact]
    public void Equals_SameIdDifferentName_ReturnsTrue()
    {
        var id = PointID.New();
        var a = CreatePoi(id, "Ayasofya");
        var b = CreatePoi(id, "Hagia Sophia");

        a.Should().Be(b);
        (a == b).Should().BeTrue();
        a.GetHashCode().Should().Be(b.GetHashCode());
    }

    [Fact]
    public void Equals_DifferentIdSameValues_ReturnsFalse()
    {
        var a = CreatePoi(PointID.New(), "Ayasofya");
        var b = CreatePoi(PointID.New(), "Ayasofya");

        a.Should().NotBe(b);
        (a != b).Should().BeTrue();
    }

    [Fact]
    public void Equals_Null_ReturnsFalse()
    {
        var a = CreatePoi(PointID.New(), "Ayasofya");

        a.Equals(null).Should().BeFalse();
        (a == null).Should().BeFalse();
    }

    [Fact]
    public void Equals_SameInstance_ReturnsTrue()
    {
        var a = CreatePoi(PointID.New(), "Ayasofya");

        a.Equals(a).Should().BeTrue();
    }

    [Fact]
    public void HashSet_SameId_StoresOnce()
    {
        var id = PointID.New();
        var set = new HashSet<POIAggregate> { CreatePoi(id, "A"), CreatePoi(id, "B") };

        set.Should().HaveCount(1);
    }

    private static POIAggregate CreatePoi(PointID id, string name)
    {
        var lat = Latitude.Create(41.0086).Value;
        var lon = Longitude.Create(28.9802).Value;
        var wkt = WellKnownText.Create($"POINT ({lon.Value} {lat.Value})").Value;
        var coordinate = CoordinateValueObject.FromLonLat(lat, lon, wkt).Value;

        return POIAggregate.Reconstitute(
            id,
            coordinate,
            PointDescription.FromString("Sultanahmet").Value,
            PointName.FromString(name).Value,
            POIStatus.ACTIVE);
    }
}