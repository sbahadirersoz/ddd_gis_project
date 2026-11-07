using Gis.Application.Features.Poi.Queries.Distance.LonLat.FindClosestPoiByLonLat;
using Gis.Application.Mapper.PoiAggregate;

using gis.Domain.Repositories;
using gis.Domain.ResultPattern;

using MediatR;

using Microsoft.Extensions.Logging;

namespace Gis.Application.Features.Poi.Queries.Distance.LonLat.FindInRangePoisByLonLat;

public class FindInRangePoisByLonLatHandler:IRequestHandler<FindInRangePoisByLonLatQuery,Result<List<FindInRangePoisByLonLatResponse>>>

{
    private readonly IPointRepository _repository;
    private readonly ILogger<FindInRangePoisByLonLatHandler> _logger;

    public FindInRangePoisByLonLatHandler(IPointRepository repository, ILogger<FindInRangePoisByLonLatHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<List<FindInRangePoisByLonLatResponse>>> Handle(FindInRangePoisByLonLatQuery request, CancellationToken cancellationToken = default)
    {
        var Lonlat = PointAggregateVOMapper.CreateLonLatFromPrimitive(longitude: request.lon, latitude: request.lat);
        if (Lonlat.IsFailure) return Result<List<FindInRangePoisByLonLatResponse>>.Failure(Lonlat.Error);
        var lon = Lonlat.Value.Item1;
        var lat = Lonlat.Value.Item2;
        var list = await _repository.FindInRangePoisByLonLatAsync(lon,lat,request.distance, cancellationToken: cancellationToken);
        var response = list.ConvertAll(x=> FindInRangePoisByLonLatResponse.FromAgg(x.agg,x.distance));
        return Result<List<FindInRangePoisByLonLatResponse>>.Success(response); 
    }
}