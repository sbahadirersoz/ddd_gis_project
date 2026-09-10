using gis.API.EndPoints;
using gis.ApplicationLayer.Features.Line.Commands.Create;
using gis.ApplicationLayer.Features.Line.Commands.Update;
using gis.ApplicationLayer.Features.Line.Queries.Distance.ID.FindClosestByIdQuery;
using gis.ApplicationLayer.Features.Line.Queries.Distance.ID.FindInRangeByIdQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using FindMostClosesCountByIdQuery =
    gis.ApplicationLayer.Features.Line.Queries.Distance.ID.FindMostClosesCountByIdQuery.FindMostClosesCountByIdQuery;

namespace gis.API.Controllers;

[ApiController]
[Route(Endpoints.Lines.Root)]
public class LineController : ControllerBase
{
    private readonly IMediator _mediator;

    public LineController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost(Endpoints.Lines.Create)]
    public async Task<IActionResult> CreateLine([FromBody] CreateLineCommand command)
    {
        var result = await _mediator.Send(command);
        return (result.IsFailure) ? BadRequest(result.Error) : Ok(result.Value);
    }

    [HttpGet(Endpoints.Lines.inRange)]
    public async Task<IActionResult> FindInRangeLines([FromQuery] FindInRangeLinesByIdQuery query)
    {
        var result = await _mediator.Send(query);
        if (result.IsFailure) return BadRequest(result.Error);
        return Ok(result.Value);
    }

    [HttpGet(Endpoints.Lines.MostClosesByCountID)]
    public async Task<IActionResult> FindClosesLinesCountById([FromQuery] FindMostClosesCountByIdQuery query)

    {
        var result = await _mediator.Send(query);
        if (result.IsFailure) return BadRequest(result.Error);
        return Ok(result.Value);
    }

    [HttpPatch(Endpoints.Lines.Update)]
    public async Task<IActionResult> UpdateLineEntity([FromBody] UpdateLineCommand command)
    {
        var result = await _mediator.Send(command);
        if (result.IsFailure) return BadRequest(result.Error);
        return Ok(result.Value);
    }

    [HttpGet(Endpoints.Lines.FindClosestById)]
    public async Task<IActionResult> FindClosestById([FromQuery] FindClosestLineByIdQuery query)
    {
        if (query.id == Guid.Empty) return BadRequest("Invalid ID");
        var result = await _mediator.Send(query);
        return (result.IsFailure) ? BadRequest(result.Error) : Ok(result.Value);
    }
}