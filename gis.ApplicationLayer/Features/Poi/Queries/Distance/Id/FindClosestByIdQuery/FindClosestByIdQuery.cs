using gis.Domain.Entities.IDs;
using gis.Domain.ResultPattern;
using MediatR;

namespace gis.ApplicationLayer.Features.Poi.Queries.FindNearestPoi;

public record FindClosestByIdQuery(Guid id):IRequest<Result<FindClosestByIdQueryResponse>>;