using FluentAssertions;

using gis.API.Common;

using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;

using Microsoft.AspNetCore.Mvc;

namespace Tests.UnitTests.API;

// Contract: Error.ToProblem() turns a domain error into an RFC 7807 ProblemDetails response.
public class ErrorToProblemTests
{
    [Theory]
    [InlineData(ErrorType.VALIDATION, 400)]
    [InlineData(ErrorType.NOT_FOUND, 404)]
    [InlineData(ErrorType.CONFLICT, 409)]
    [InlineData(ErrorType.UNEXPECTED, 500)]
    public void ToProblem_MapsErrorTypeToStatusCode(ErrorType type, int expectedStatus)
    {
        var error = new Error("Some.Code", "SOME_DESC", type);

        var result = error.ToProblem();

        result.StatusCode.Should().Be(expectedStatus);
        result.Value.Should().BeOfType<ProblemDetails>()
            .Which.Status.Should().Be(expectedStatus);
    }

    [Fact]
    public void ToProblem_PutsCodeInTitleAndDescInDetail()
    {
        var error = new Error("RepositoryErrors.Entity.NotFound", "ENTITY_NOT_FOUND", ErrorType.NOT_FOUND);

        var problem = (ProblemDetails)error.ToProblem().Value!;

        problem.Title.Should().Be("RepositoryErrors.Entity.NotFound");
        problem.Detail.Should().Be("ENTITY_NOT_FOUND");
    }

    [Fact]
    public void Error_DefaultsToValidation()
    {
        new Error("c", "d").type.Should().Be(ErrorType.VALIDATION);
    }

    [Fact]
    public void EntityNotFound_Is404()
    {
        RepositoryErrors.ENTITY_NOT_FOUND.ToProblem().StatusCode.Should().Be(404);
    }

    [Fact]
    public void AlreadyExist_Is409()
    {
        RepositoryErrors.VALUE_ALREADY_EXIST_IN_DB.ToProblem().StatusCode.Should().Be(409);
    }

    [Fact]
    public void Error_HasNoStackTrace_SoItCannotLeakToClients()
    {
        typeof(Error).GetProperty("StackTrace").Should().BeNull();
    }
}
