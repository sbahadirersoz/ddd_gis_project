using gis.Domain.Entities.Information;
using gis.Domain.ResultPattern;
using MediatR;

namespace Gis.Application.Features.Poi.Queries.Distance.FindNearestPoiByName;

public record FindClosestPoiByNameQuery(string name):IRequest<Result<FindClosestPoiByNameQueryResponse>>
{
}