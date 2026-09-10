using gis.Domain.Entities.Information;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;
using MediatR;
using Microsoft.Extensions.Logging;

namespace gis.ApplicationLayer.Features.Poi.Queries.Distance.FindNearestPoiByName;

public class FindClosestPoiByNameQueryHandler(
    ILogger<FindClosestPoiByNameQueryHandler> logger,
    IPointRepository repository)
    : IRequestHandler<FindClosestPoiByNameQuery,Result< FindClosestPoiByNameQueryResponse>>
{
    private readonly ILogger<FindClosestPoiByNameQueryHandler> _logger = logger;

    public async Task <Result<FindClosestPoiByNameQueryResponse>> Handle(FindClosestPoiByNameQuery request, CancellationToken cancellationToken)
    {

        var name = PointName.FromString(request.name);
        if (name.IsFailure) return Result<FindClosestPoiByNameQueryResponse>.Failure(name.Error);
        var entity = await repository.FindClosestPoiByName(name.Value, cancellationToken: cancellationToken);
         if (entity == null) return Result<FindClosestPoiByNameQueryResponse>.Failure(RepositoryErrors.QUERY_RETURNS_EMPTY);
         var result = FindClosestPoiByNameQueryResponse.FromAgg(entity.Value.agg,entity.Value.distance);
         return Result<FindClosestPoiByNameQueryResponse>.Success(result);
    }
}