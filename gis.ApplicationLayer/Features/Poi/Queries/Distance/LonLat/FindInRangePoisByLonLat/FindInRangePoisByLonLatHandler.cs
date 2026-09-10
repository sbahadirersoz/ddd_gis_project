using gis.ApplicationLayer.Features.Poi.Queries.Distance.LonLat.FindClosestPoiByLonLat;
using gis.ApplicationLayer.Mapper.PoiAggregate;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;
using MediatR;
using Microsoft.Extensions.Logging;

namespace gis.ApplicationLayer.Features.Poi.Queries.Distance.LonLat.FindInRangePoisByLonLat;

public class FindInRangePoisByLonLatHandler:IRequestHandler<FindInRangePoisByLonLatQuery,Result<List<FindInRangePoisByLonLatResponse>>>

{
    private readonly IPointRepository _repository;
    private readonly ILogger<FindInRangePoisByLonLatHandler> _logger;

    public FindInRangePoisByLonLatHandler(IPointRepository repository, ILogger<FindInRangePoisByLonLatHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<List<FindInRangePoisByLonLatResponse>>> Handle(FindInRangePoisByLonLatQuery request, CancellationToken cancellationToken)
    {
        var Lonlat = PointAggregateVOMapper.CreateLonLatFromPrimitive(request.lat,request.lon);
        if (Lonlat.IsFailure) return Result<List<FindInRangePoisByLonLatResponse>>.Failure(Lonlat.Error);
        var lon = Lonlat.Value.Item1;
        var lat = Lonlat.Value.Item2;
        var list = await _repository.FindInRangePoisByLonLatAsync(lon,lat,request.distance, cancellationToken: cancellationToken);
        if (  (list == null ||list.Count == 0)) return Result<List<FindInRangePoisByLonLatResponse>>.Failure(RepositoryErrors.QUERY_RETURNS_EMPTY);
        var response = list.ConvertAll(x=> FindInRangePoisByLonLatResponse.FromAgg(x.agg,x.distance));
        return Result<List<FindInRangePoisByLonLatResponse>>.Success(response); 
    }
}