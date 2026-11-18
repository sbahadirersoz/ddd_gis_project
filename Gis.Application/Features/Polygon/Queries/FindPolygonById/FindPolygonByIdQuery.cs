using gis.Domain.ResultPattern;

using MediatR;

namespace Gis.Application.Features.Polygon.Queries.FindPolygonById;

public record FindPolygonByIdQuery(Guid id) : IRequest<Result<FindPolygonByIdResponse>>;
