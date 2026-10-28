using gis.Domain.Entities.IDs;
using gis.Domain.ResultPattern;
using MediatR;

namespace Gis.Application.Features.Poi.Queries.FindNearestPoi;

public record FindClosestByIdQuery(Guid id):IRequest<Result<FindClosestByIdQueryResponse>>;