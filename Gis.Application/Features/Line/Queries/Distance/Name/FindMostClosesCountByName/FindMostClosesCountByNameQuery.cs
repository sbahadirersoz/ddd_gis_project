using gis.Domain.ResultPattern;
using MediatR;

namespace Gis.Application.Features.Line.Queries.Distance.Name.FindMostClosesCountByName;

public record FindMostClosesCountByNameQuery(string Name , int count):IRequest<Result<List<FindMostClosesCountByNameResponse>>>;