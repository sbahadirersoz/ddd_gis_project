using gis.Domain.ResultPattern;
using MediatR;

namespace Gis.Application.Features.Line.Queries.Distance.ID.FindClosestByIdQuery;

public record FindClosestLineByIdQuery(Guid id):IRequest<Result<FindClosestLineByIdResponse>>;