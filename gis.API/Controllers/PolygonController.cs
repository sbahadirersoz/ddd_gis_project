using gis.API.Common;
using gis.API.EndPoints;

using Gis.Application.Features.Polygon.Commands;
using Gis.Application.Features.Polygon.Commands.AddHoleToPolygon;
using Gis.Application.Features.Polygon.Queries.FindPolygonById;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace gis.API.Controllers;

[ApiController]
[Route(Endpoints.Polygons.Root)]
public class PolygonController : ControllerBase
{
    private readonly IMediator _mediator;

    public PolygonController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost(Endpoints.Polygons.Create)]
    public async Task<IActionResult> CreatePolygon([FromBody] CreatePolygonCommand command)
    {
        var result = await _mediator.Send(command);
        return (result.IsFailure) ? result.Error.ToProblem() : Ok(result.Value);
    }

    [HttpGet(Endpoints.Polygons.GetById)]
    public async Task<IActionResult> GetByIdPolygon([FromRoute] FindPolygonByIdQuery query)
    {
        var result = await _mediator.Send(query);
        return (result.IsFailure) ? result.Error.ToProblem() : Ok(result.Value);
    }

    [HttpPost(Endpoints.Polygons.AddHole)]
    public async Task<IActionResult> AddHole([FromRoute] Guid id, [FromBody] AddHoleToPolygonCommand command)
    {
        var result = await _mediator.Send(command with { id = id });
        return (result.IsFailure) ? result.Error.ToProblem() : Ok(result.Value);
    }
}