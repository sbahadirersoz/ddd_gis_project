using gis.Domain.Entities.WKT;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;
using MediatR;
using Microsoft.Extensions.Logging;

namespace gis.ApplicationLayer.Features.Line.Queries.Distance.WKT.FindMostClosesCountByWKT;

public class FindMostClosesCountByWKTHandler : IRequestHandler<FindMostClosesLineCountByWKTQuery,Result<List<FindMostClosesLineCountByWKTResponse>>>
{
    private readonly ILogger<FindMostClosesCountByWKTHandler> _logger;
    private readonly ILineRepository _repository;

    public FindMostClosesCountByWKTHandler(ILogger<FindMostClosesCountByWKTHandler> logger, ILineRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<Result<List<FindMostClosesLineCountByWKTResponse>>> Handle(FindMostClosesLineCountByWKTQuery request, CancellationToken cancellationToken)
    {
        var wkt = WellKnownText.Create(request.wkt);
        if (wkt.IsFailure) return Result<List<FindMostClosesLineCountByWKTResponse>>.Failure(wkt.Error);
        
        var list = await _repository.FindMostClosesCountByWktAsync(wkt.Value,request.count, cancellationToken: cancellationToken);
        if (list.Count == 0) return Result<List<FindMostClosesLineCountByWKTResponse>>.Failure(RepositoryErrors.QUERY_RETURNS_EMPTY);
        _logger.LogInformation("Query Found Count: {count}", list.Count);
        
        var response = list.ConvertAll(x=> FindMostClosesLineCountByWKTResponse.FromAgg(x.agg,x.distance));
        return Result<List<FindMostClosesLineCountByWKTResponse>>.Success(response);
        
    }
}