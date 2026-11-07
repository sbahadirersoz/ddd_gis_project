using FluentAssertions;

using Gis.Application.Common.Validators.QueryValidators;

using gis.Domain.ResultPattern.Errors;

namespace Tests.UnitTests.Application.Common.Validators.QueryValidators;

public class CountValidatorTests
{
    private readonly CountValidator _validator = new();

    [Theory]
    [InlineData(1)]
    [InlineData(12)]
    public void VALIDATION_IS_VALID(int count)
    {
        var res = _validator.Validate(count);
        res.IsValid.Should().BeTrue();
    }
    [Theory]
    [InlineData(-1)]
    [InlineData(-2)]
    public void VALIDATION_IS_INVALID(int count)
    {
        var res = _validator.Validate(count);
        res.IsValid.Should().BeFalse();
    }
    
    
    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(13)]
    public void Validate_CountOutOfRange_ReturnsSingleInvalidCountError(int count)
    {
        var result = _validator.Validate(count);

        result.Errors.Should().ContainSingle()
            .Which.ErrorCode.Should().Be(BusinessRuleErrors.DistanceQueryRulesErrors.INVALID_QUERY_COUNT.Code);
    }
}