using FluentAssertions;

using Gis.Application.Features.Line.Queries.Distance.ID.FindMostClosesCountByIdQuery;

using gis.Domain.ResultPattern.Errors;

namespace Tests.UnitTests.Application.Features.Line.ValidatorTests;

public class FindMostClosesCountByIdValidatorTest
{
    private readonly FindMostClosesCountByIdValidator _validator = new();


    [InlineData(13)]
    [InlineData(0)]
    [InlineData(-1)]
    [Theory]
    public void CHECK_VALIDATOR_INVALID(int count)
    {
        var validator = _validator.Validate(new FindMostClosesCountByIdQuery(Guid.NewGuid(), count));
        validator.Errors.Should().ContainSingle().Which.ErrorCode.Should()
            .Be(BusinessRuleErrors.DistanceQueryRulesErrors.INVALID_QUERY_COUNT.Code);
    }

    [InlineData(1)]
    [InlineData(12)]
    [Theory]
    public void  CHECK_VALIDATOR_VALID(int count)
    {
        var validator = _validator.Validate(new FindMostClosesCountByIdQuery(Guid.NewGuid(), count));
        validator.IsValid.Should().BeTrue();
    }
}