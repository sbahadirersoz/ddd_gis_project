using Gis.Application.Mapper.PoiAggregate;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Gis.Application.Features.Poi.Queries.Distance.LonLat.FindClosestPoiByLonLat;

public class FindClosestPoiByLonLatHandler:IRequestHandler<FindClosestPoiByLonLatQuery,Result<FindClosestPoiByLonLatResponse>>
{
    private readonly ILogger<FindClosestPoiByLonLatHandler> _logger;
    private readonly IPointRepository _repository;

    public FindClosestPoiByLonLatHandler(ILogger<FindClosestPoiByLonLatHandler> logger, IPointRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<Result<FindClosestPoiByLonLatResponse>> Handle(FindClosestPoiByLonLatQuery request, CancellationToken cancellationToken)
    {
        double latVal = request.lat.Value;
        double lonVal = request.lon.Value;
        var lonLatFromPrimitive = PointAggregateVOMapper.CreateLonLatFromPrimitive(longitude: lonVal, latitude: latVal);
        if (lonLatFromPrimitive.IsFailure) return  Result<FindClosestPoiByLonLatResponse>.Failure(lonLatFromPrimitive.Error);
        var lon = lonLatFromPrimitive.Value.Item1;
        var lat = lonLatFromPrimitive.Value.Item2;
        var entity = await _repository.FindClosestPoiByGivenLonLatAsync(lon,lat, cancellationToken: cancellationToken);
        if (entity.agg == null ) return Result<FindClosestPoiByLonLatResponse>.Failure(RepositoryErrors.NO_NEIGHBOR_FOUND);
        var response = FindClosestPoiByLonLatResponse.FromAgg(entity.agg,entity.distance);
        return Result<FindClosestPoiByLonLatResponse>.Success(response);
    }
}