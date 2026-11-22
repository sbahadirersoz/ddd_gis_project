using Gis.Application.Dtos;
using gis.Domain.ResultPattern;
using MediatR;

namespace Gis.Application.Features.Polygon.Commands.AddHoleToPolygon;

public record AddHoleToPolygonCommand(Guid id, List<CoordinateDto> hole) : IRequest<Result<AddHoleToPolygonResponse>>;
