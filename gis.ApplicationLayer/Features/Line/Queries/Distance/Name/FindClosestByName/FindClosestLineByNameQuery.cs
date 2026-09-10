using gis.Domain.ResultPattern;
using MediatR;

namespace gis.ApplicationLayer.Features.Line.Queries.Distance.Name.FindClosestByName;

public record FindClosestLineByNameQuery(string name):IRequest<Result<FindClosestLineByNameResponse>>;