using gis.Domain.ResultPattern;
using MediatR;

namespace gis.ApplicationLayer.Features.Line.Queries.Distance.ID.FindMostClosesCountByIdQuery;

public record FindMostClosesCountByIdQuery(Guid id,int count):IRequest<Result<List<FindMostClosesCountByIdResponse>>>;