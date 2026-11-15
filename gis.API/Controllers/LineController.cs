using gis.API.EndPoints;
using Gis.Application.Features.Line.Commands.Create;
using Gis.Application.Features.Line.Commands.Delete;
using Gis.Application.Features.Line.Commands.Update;
using Gis.Application.Features.Line.Queries.Distance.ID.FindClosestByIdQuery;
using Gis.Application.Features.Line.Queries.Distance.ID.FindInRangeByIdQuery;
using Gis.Application.Features.Line.Queries.Distance.LonLat.FindClosestByLonLat;
using Gis.Application.Features.Line.Queries.Distance.LonLat.FindInRangeByLonLat;
using Gis.Application.Features.Line.Queries.Distance.LonLat.FindMostClosesCountByLonLat;
using Gis.Application.Features.Line.Queries.FindLineByID;
using Gis.Application.Dtos;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using FindMostClosesCountByIdQuery =
    Gis.Application.Features.Line.Queries.Distance.ID.FindMostClosesCountByIdQuery.FindMostClosesCountByIdQuery;

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
    [HttpDelete(Endpoints.Lines.Delete)]
    public async Task<IActionResult> SoftDeleteById([FromRoute] DeleteLineByIdCommand cmd)
    {

        var result = await _mediator.Send(cmd);
        return (result.IsFailure) ? BadRequest(result.Error) : Ok(result.Value);
    }
    // lon/lat are nullable so a missing one is a 400, not a silent 0 (the equator)
    private const string LonLatRequired = "longitude and latitude are required";

    [HttpGet(Endpoints.Lines.FindClosestByLatLon)]
    public async Task<IActionResult> FindClosestByLonLat([FromQuery] double? longitude, [FromQuery] double? latitude)
    {
        if (longitude is null || latitude is null) return BadRequest(LonLatRequired);
        var result = await _mediator.Send(new FindClosestLineByLonLatQuery(new CoordinateDto(longitude.Value, latitude.Value)));
        return result.IsFailure ? BadRequest(result.Error) : Ok(result.Value);
    }

    [HttpGet(Endpoints.Lines.inRangeByLatLon)]
    public async Task<IActionResult> FindInRangeByLonLat([FromQuery] double? longitude, [FromQuery] double? latitude,
        [FromQuery] double DistanceByMeters)
    {
        if (longitude is null || latitude is null) return BadRequest(LonLatRequired);
        var result = await _mediator.Send(new FindInRangeLineByLonLatQuery(new CoordinateDto(longitude.Value, latitude.Value), DistanceByMeters));
        return result.IsFailure ? BadRequest(result.Error) : Ok(result.Value);
    }

    [HttpGet(Endpoints.Lines.MostClosesByCountLatLon)]
    public async Task<IActionResult> FindMostClosesCountByLonLat([FromQuery] double? longitude, [FromQuery] double? latitude,
        [FromQuery] int count)
    {
        if (longitude is null || latitude is null) return BadRequest(LonLatRequired);
        var result = await _mediator.Send(new FindMostClosesLineCountByLonLatQuery(new CoordinateDto(longitude.Value, latitude.Value), count));
        return result.IsFailure ? BadRequest(result.Error) : Ok(result.Value);
    }

    [HttpGet(Endpoints.Lines.GetById)]
    public async Task<IActionResult> GetById([FromRoute] FindLineByIDQuery query)
    {

        var result = await _mediator.Send(query);
        return (result.IsFailure) ? BadRequest(result.Error) : Ok(result.Value);
    }
    
}