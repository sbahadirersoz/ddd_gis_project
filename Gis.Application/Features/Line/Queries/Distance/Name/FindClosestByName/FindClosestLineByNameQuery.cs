using gis.Domain.ResultPattern;
using MediatR;

namespace Gis.Application.Features.Line.Queries.Distance.Name.FindClosestByName;

public record FindClosestLineByNameQuery(string name):IRequest<Result<FindClosestLineByNameResponse>>;