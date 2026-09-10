using gis.ApplicationLayer.Mapper.LineMapper;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern;
using MediatR;
using Microsoft.Extensions.Logging;

namespace gis.ApplicationLayer.Features.Line.Queries.Distance.LonLat.FindMostClosesCountByLonLat;

public class FindMostClosesLineCountByLonLatHandler:IRequestHandler<FindMostClosesLineCountByLonLatQuery,Result<List<FindMostClosesLineCountByLonLatResponse>>>
{
    
    private readonly ILogger<FindMostClosesLineCountByLonLatHandler> _logger;
    private readonly ILineRepository _lineRepository;

    public FindMostClosesLineCountByLonLatHandler(ILogger<FindMostClosesLineCountByLonLatHandler> logger, ILineRepository lineRepository)
    {
        _logger = logger;
        _lineRepository = lineRepository;
    }

    public async Task<Result<List<FindMostClosesLineCountByLonLatResponse>>> Handle(FindMostClosesLineCountByLonLatQuery request, CancellationToken cancellationToken)
    {
        var primitivesToCordinate = LineAggregateVOMapper.PrimitivesToCordinate(request.lonLat.Latitude,request.lonLat.Longitude);
        
        _logger.LogInformation("Check PrimativeMapper IsFailure");
        if (primitivesToCordinate.IsFailure)
            return Result<List<FindMostClosesLineCountByLonLatResponse>>.Failure(primitivesToCordinate.Error);

        var lat = primitivesToCordinate.Value.Latitude;
        var lon = primitivesToCordinate.Value.Longitude;
        var list = await _lineRepository.FindMostClosesCountByLonLatAsync(lon,lat,request.count, cancellationToken: cancellationToken);
        var response = list.ConvertAll(x=> FindMostClosesLineCountByLonLatResponse.FromAgg(x.agg,x.distance));
        return Result<List<FindMostClosesLineCountByLonLatResponse>>.Success(response);
    }
}