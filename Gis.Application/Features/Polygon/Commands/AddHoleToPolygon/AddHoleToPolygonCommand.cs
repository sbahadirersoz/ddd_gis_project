using gis.Domain.ResultPattern;
using MediatR;

namespace Gis.Application.Features.Polygon.Commands.AddHoleToPolygon;

public record AddHoleToPolygonCommand() : IRequest<Result<AddHoleToPolygonResponse>>;