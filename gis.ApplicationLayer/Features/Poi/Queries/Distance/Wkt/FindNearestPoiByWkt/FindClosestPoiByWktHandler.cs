using gis.Domain.Entities.WKT;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;
using MediatR;
using Microsoft.Extensions.Logging;

namespace gis.ApplicationLayer.Features.Poi.Queries.Distance.Wkt.FindNearestPoiByWkt;

public class FindClosestPoiByWktHandler : IRequestHandler<FindClosestPoiByWktQuery, Result<FindClosestPoiByWktResponse>>
{
    private readonly IPointRepository _repository;
    private readonly ILogger<FindClosestPoiByWktHandler> _logger;

    public FindClosestPoiByWktHandler(IPointRepository repository, ILogger<FindClosestPoiByWktHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<FindClosestPoiByWktResponse>> Handle(FindClosestPoiByWktQuery request,
        CancellationToken cancellationToken)
    {
        var wkt = WellKnownText.Create(request.wkt);
        if (wkt.IsFailure) return Result<FindClosestPoiByWktResponse>.Failure(wkt.Error);
        
        var closest = await _repository.FindClosestPoiByWktAsync(wkt.Value, cancellationToken: cancellationToken);
        if (closest.agg == null) return Result<FindClosestPoiByWktResponse>.Failure(RepositoryErrors.VALUE_ALREADY_EXIST_IN_DB);
        var response = FindClosestPoiByWktResponse.CreateFromAgg(closest.agg, closest.distance);
        return Result<FindClosestPoiByWktResponse>.Success(response);
    }
}