using gis.ApplicationLayer.Features.Poi.Queries.Distance.Id.FindInRangePoisById;
using gis.Domain.ResultPattern;
using MediatR;

namespace gis.ApplicationLayer.Features.Poi.Queries.FindPoisInGivenRange;

public record FindMostClosesCountByIdQuery(Guid id, int count) : IRequest<Result<List<FindMostClosesCountByIdQueryResponse>>>;