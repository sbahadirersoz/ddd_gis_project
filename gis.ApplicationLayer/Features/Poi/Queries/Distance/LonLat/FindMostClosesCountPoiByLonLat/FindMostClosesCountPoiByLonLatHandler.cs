using gis.ApplicationLayer.Mapper.PoiAggregate;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;
using MediatR;
using Microsoft.Extensions.Logging;

namespace gis.ApplicationLayer.Features.Poi.Queries.Distance.LonLat.FindMostClosesCountPoiByLonLat;

public class FindMostClosesCountPoiByLonLatHandler:IRequestHandler<FindMostClosesCountPoiByLonLatQuery,Result<List<FindMostClosesCountPoiByLonLatResponse>>>
{
    private readonly IPointRepository _repository;
    private readonly ILogger<FindMostClosesCountPoiByLonLatHandler>_logger;

    public FindMostClosesCountPoiByLonLatHandler(IPointRepository repository, ILogger<FindMostClosesCountPoiByLonLatHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<List<FindMostClosesCountPoiByLonLatResponse>>> Handle(FindMostClosesCountPoiByLonLatQuery request, CancellationToken cancellationToken)
    {
        var lonlat = PointAggregateVOMapper.CreateLonLatFromPrimitive(request.lat, request.lon);
        if (lonlat.IsFailure) return Result<List<FindMostClosesCountPoiByLonLatResponse>>.Failure(lonlat.Error);
        var lon = lonlat.Value.Item1;
        var lat = lonlat.Value.Item2;
        var list = await _repository.FindMostClosesCountByLonLatAsync(lon,lat,request.count, cancellationToken: cancellationToken);
        if (list == null || list.Count == 0)
            return Result<List<FindMostClosesCountPoiByLonLatResponse>>.Failure(RepositoryErrors.QUERY_RETURNS_EMPTY);
        var responses = list.ConvertAll(x=> FindMostClosesCountPoiByLonLatResponse.FromAgg(x.agg,x.distance));
        return Result<List<FindMostClosesCountPoiByLonLatResponse>>.Success(responses);
    }
}