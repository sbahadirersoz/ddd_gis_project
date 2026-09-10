using gis.Domain.ResultPattern;
using MediatR;

namespace gis.ApplicationLayer.Features.Line.Queries.Distance.ID.FindClosestByIdQuery;

public record FindClosestLineByIdQuery(Guid id):IRequest<Result<FindClosestLineByIdResponse>>;