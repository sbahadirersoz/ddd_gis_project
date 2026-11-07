using gis.Domain.Contracts;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Gis.Application.Features.Poi.Queries.Distance.Wkt.FindNearestPoiByWkt;

public class FindClosestPoiByWktHandler : IRequestHandler<FindClosestPoiByWktQuery, Result<FindClosestPoiByWktResponse>>
{
    private readonly IPointRepository _repository;
    private readonly ILogger<FindClosestPoiByWktHandler> _logger;
    private readonly ITopologySuiteWKTContract _wktContract;

    public FindClosestPoiByWktHandler(IPointRepository repository, ILogger<FindClosestPoiByWktHandler> logger, ITopologySuiteWKTContract wktContract)
    {
        _repository = repository;
        _logger = logger;
        _wktContract = wktContract;
    }

    public async Task<Result<FindClosestPoiByWktResponse>> Handle(FindClosestPoiByWktQuery request,
        CancellationToken cancellationToken  = default)
    {
        var wkt = _wktContract.ParsePointWkt(request.wkt);
        if (wkt.IsFailure) return Result<FindClosestPoiByWktResponse>.Failure(wkt.Error);
        
        var closest = await _repository.FindClosestPoiByWktAsync(wkt.Value, cancellationToken: cancellationToken);
        if (closest.agg == null) return Result<FindClosestPoiByWktResponse>.Failure(RepositoryErrors.NO_NEIGHBOR_FOUND);
        var response = FindClosestPoiByWktResponse.CreateFromAgg(closest.agg, closest.distance);
        return Result<FindClosestPoiByWktResponse>.Success(response);
    }
}