using FluentAssertions;

using Gis.Application.Common.Validators.QueryValidators;

using gis.Domain.ResultPattern.Errors;

namespace Tests.UnitTests.Application.Common.Validators.QueryValidators;

public class RangeValidatorTests
{
    private readonly   RangeValidator _validator = new RangeValidator();

    [Theory]
    [InlineData(500)]
    [InlineData(1000)]
    [InlineData(2000)]
    public void VALIDATION_IS_VALID(double range)
    {

        var res = _validator.Validate(range);
        res.IsValid.Should().BeTrue();
    }
    [Theory]
    [InlineData(-500)]
    [InlineData(-1000)]
    [InlineData(-2000)]
    public void VALIDATION_IS_INVALID(double range)
    {
        var res = _validator.Validate(range);
        res.IsValid.Should().BeFalse();
        res.Errors.Should().ContainSingle().Which.ErrorCode.Should()
            .Be(BusinessRuleErrors.DistanceQueryRulesErrors.INVALID_DISTANCE_RANGE.Code);
    }
}