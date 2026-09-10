using gis.ApplicationLayer.Mapper.LineMapper;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;
using MediatR;
using Microsoft.Extensions.Logging;

namespace gis.ApplicationLayer.Features.Line.Queries.Distance.LonLat.FindInRangeByLonLat;

public class FindInRangeLineByLonLatHandler:IRequestHandler<FindInRangeLineByLonLatQuery,Result<List<FindInRangeLineByLonLatResponse>>>
{ 
    
    private readonly ILogger<FindInRangeLineByLonLatHandler> _logger;
    private ILineRepository _lineRepository;


    public FindInRangeLineByLonLatHandler(ILogger<FindInRangeLineByLonLatHandler> logger, ILineRepository lineRepository)
    {
        _logger = logger;
        _lineRepository = lineRepository;
    }

    public async Task<Result<List<FindInRangeLineByLonLatResponse>>> Handle(FindInRangeLineByLonLatQuery request, CancellationToken cancellationToken)
    {
        var primitivesToCordinateResult = LineAggregateVOMapper.PrimitivesToCordinate(request.LonLat.Latitude,request.LonLat.Longitude);
        if (primitivesToCordinateResult.IsFailure)
            return Result<List<FindInRangeLineByLonLatResponse>>.Failure(primitivesToCordinateResult.Error);
        var lat = primitivesToCordinateResult.Value.Latitude;
        var lon = primitivesToCordinateResult.Value.Longitude;
        
        _logger.LogInformation("primitives Passed Successful");

        var list = await _lineRepository.FindInRangeLinesByLonLatAsync( lon,lat,request.DistanceInMeter, cancellationToken: cancellationToken);
        if (list.Count == 0) return Result<List<FindInRangeLineByLonLatResponse>>.Failure(RepositoryErrors.QUERY_RETURNS_EMPTY);
        
        var response = list.ConvertAll(x=> FindInRangeLineByLonLatResponse.FromAgg(x.agg,x.distance));
        return Result<List<FindInRangeLineByLonLatResponse>>.Success(response);
    }
}