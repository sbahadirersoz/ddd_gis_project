using gis.Domain.ResultPattern;
using MediatR;

namespace Gis.Application.Features.Line.Queries.Distance.ID.FindMostClosesCountByIdQuery;

public record FindMostClosesCountByIdQuery(Guid id,int count):IRequest<Result<List<FindMostClosesCountByIdResponse>>>;