using gis.Domain.ResultPattern;

using Microsoft.AspNetCore.Mvc;

namespace gis.API.Common;

public static class ErrorExtensions
{
    public static ObjectResult ToProblem(this Error error)
    {
        var status = (int)error.type;

        var problem = new ProblemDetails
        {
            Status = status,
            Title = error.Code,
            Detail = error.Desc
        };

        return new ObjectResult(problem) { StatusCode = status };
    }
}