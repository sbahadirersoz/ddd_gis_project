using FluentAssertions;

using FluentValidation.Results;

using Gis.Application.Features.Poi.Queries.Distance.LonLat.FindClosestPoiByLonLat;

using gis.Domain.ResultPattern.Errors;

using NSubstitute;
using NSubstitute.ReceivedExtensions;

namespace Tests.UnitTests.Application.Features.Poi.Queries.FindClosestByLonLat;

public class FindClosestPoiByLonLatValidatorTests
{
    private readonly FindClosestPoiByLonLatValidator _validator = new();


    [Theory]
    [InlineData(null, 25d)]
    [InlineData(30d, null)]
    public void SHOULD_HAVE_ERROR(double? lon, double? lat)
    {
        var query = new FindClosestPoiByLonLatQuery(lon, lat);
        var validation = _validator.Validate(query);
        validation.Errors.Should().ContainSingle()
            .Which.ErrorCode.Should().Be(BusinessRuleErrors.DistanceQueryRulesErrors.MISSING_COORDINATE.Code);
    }

    [Theory]
    [InlineData(20d, 25d)]
    [InlineData(30d, 22d)]
    public void SHOULD_SUCCESS(double? lon, double? lat)
    {
        var query = new FindClosestPoiByLonLatQuery(lon, lat);
        var validation = _validator.Validate(query);
        validation.IsValid.Should().BeTrue();
    }

}