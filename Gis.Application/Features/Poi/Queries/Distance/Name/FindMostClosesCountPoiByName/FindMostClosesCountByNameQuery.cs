using gis.Domain.Entities.Information;
using gis.Domain.ResultPattern;
using MediatR;

namespace Gis.Application.Features.Poi.Queries.Distance.FindNearestCountPoiByName;

public record FindMostClosesCountByNameQuery(string name,int count):IRequest<Result<List<FindMostClosesCountByNameQueryResponse>>>;