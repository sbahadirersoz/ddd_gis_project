using gis.Domain.ResultPattern;
using MediatR;

namespace gis.ApplicationLayer.Features.Polygon.Commands.AddHoleToPolygon;

public record AddHoleToPolygonCommand() : IRequest<Result<AddHoleToPolygonResponse>>;