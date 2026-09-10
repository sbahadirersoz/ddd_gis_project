using gis.Domain.Entities.WKT;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;
using MediatR;
using Microsoft.Extensions.Logging;

namespace gis.ApplicationLayer.Features.Line.Queries.Distance.WKT.FindInRangeByWKT;

public class FindInRangeByWKTHandler:IRequestHandler<FindInRangeLineByWKTQuery,Result<List<FindInRangeLineByWKTResponse> >>
{
    
    private readonly  ILogger<FindInRangeByWKTHandler> _logger;
    private readonly  ILineRepository _repository;

    public FindInRangeByWKTHandler(ILogger<FindInRangeByWKTHandler> logger, ILineRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<Result<List<FindInRangeLineByWKTResponse>>> Handle(FindInRangeLineByWKTQuery request, CancellationToken cancellationToken)
    {
        var wkt = WellKnownText.Create(request.wkt);
        if (wkt.IsFailure) return Result<List<FindInRangeLineByWKTResponse>>.Failure(wkt.Error);
        var list = await _repository.FindInRangeLinesByWktAsync(wkt.Value,request.distance, cancellationToken: cancellationToken);
        if (list.Count == 0) return Result<List<FindInRangeLineByWKTResponse>>.Failure(RepositoryErrors.QUERY_RETURNS_EMPTY);
        
        _logger.Log(LogLevel.Information,"Query Found  Count : {List}",list.Count);
        var response = list.ConvertAll(x=> FindInRangeLineByWKTResponse.FromAgg(x.agg,x.distance));
        return Result<List<FindInRangeLineByWKTResponse>>.Success(response);
    }
}