using Gis.Application.Dtos;
using gis.Domain.ResultPattern;
using MediatR;

namespace Gis.Application.Features.Polygon.Commands;

public record CreatePolygonCommand(List<CoordinateDto> coords, string Name,string? desc ):IRequest<Result<CreatePolygonCommandResponse>>;