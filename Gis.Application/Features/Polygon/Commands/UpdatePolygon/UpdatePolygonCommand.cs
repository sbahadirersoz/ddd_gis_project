using gis.Domain.Entities.Information.Polygon;
using gis.Domain.ResultPattern;
using MediatR;

namespace Gis.Application.Features.Polygon.Commands.UpdatePolygon;

public record UpdatePolygonCommand(Guid id, string? name = null, string? desc = null, PolygonStatus? status = null)
    : IRequest<Result<UpdatePolygonCommandResponse>>;
