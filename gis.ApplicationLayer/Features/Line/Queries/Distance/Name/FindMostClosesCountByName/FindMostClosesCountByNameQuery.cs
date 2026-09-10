using gis.Domain.ResultPattern;
using MediatR;

namespace gis.ApplicationLayer.Features.Line.Queries.Distance.Name.FindMostClosesCountByName;

public record FindMostClosesCountByNameQuery(string Name , int count):IRequest<Result<List<FindMostClosesCountByNameResponse>>>;