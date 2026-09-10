using gis.ApplicationLayer.Dtos;
using gis.Domain.ResultPattern;
using MediatR;

namespace gis.ApplicationLayer.Features.Polygon.Commands;

public record CreatePolygonCommand(List<CoordinateDto> coords, string Name,string? desc ):IRequest<Result<CreatePolygonCommandResponse>>;