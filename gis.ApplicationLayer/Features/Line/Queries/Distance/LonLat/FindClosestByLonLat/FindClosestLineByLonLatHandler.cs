using gis.ApplicationLayer.Mapper.LineMapper;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern;
using MediatR;
using Microsoft.Extensions.Logging;

namespace gis.ApplicationLayer.Features.Line.Queries.Distance.LonLat.FindClosestByLonLat;

public class FindClosestLineByLonLatHandler:IRequestHandler<FindClosestLineByLonLatQuery,Result<FindClosestLineByLonLatResponse>>
{
    private  readonly  ILogger<FindClosestLineByLonLatHandler> _logger;
    private  readonly  ILineRepository _repository;

    public FindClosestLineByLonLatHandler(ILogger<FindClosestLineByLonLatHandler> logger, ILineRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<Result<FindClosestLineByLonLatResponse>> Handle(FindClosestLineByLonLatQuery request, CancellationToken cancellationToken)
    {
        var primitivesToCordinate = LineAggregateVOMapper.PrimitivesToCordinate(request.LonLat.Latitude,request.LonLat.Longitude);
        if (primitivesToCordinate.IsFailure) return Result<FindClosestLineByLonLatResponse>.Failure(primitivesToCordinate.Error);
        
        
        var lat = primitivesToCordinate.Value.Latitude;
        var lon = primitivesToCordinate.Value.Longitude;
        
        
        var findClosestByLonLatAsync = await _repository.FindClosestByLonLatAsync(lon,lat, cancellationToken: cancellationToken);
        var response = FindClosestLineByLonLatResponse.FromAgg(findClosestByLonLatAsync.agg, findClosestByLonLatAsync.distance);
        return Result<FindClosestLineByLonLatResponse>.Success(response);
    }
}