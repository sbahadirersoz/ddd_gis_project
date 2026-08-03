using FluentAssertions;
using gis.Domain.Entities.Information;
using gis.Domain.ResultPattern.Errors;

namespace Tests.Entities.Information;

public class PointNameTest
{

    [Fact]
    public void METHOD()
    {
        var name =  PointName.FromString("Mock Location");
        name.Error.Should().Be(DomainErrors.POIErrors.PointName.BAD_CREDENTIALS_FOR_POINT_NAME);
    }
}