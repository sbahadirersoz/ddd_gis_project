using gis.Domain.Entities.WKT;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;
using MediatR;
using Microsoft.Extensions.Logging;

namespace gis.ApplicationLayer.Features.Line.Queries.Distance.WKT.FindClosestByWKT;

public class FindClosestByWKTHandler:IRequestHandler<FindClosestLineByWKTQuery,Result< FindClosestLineByWKTResponse>>
{
    
    private readonly ILogger<FindClosestByWKTHandler> _logger;
    private readonly ILineRepository _repository;

    public FindClosestByWKTHandler(ILogger<FindClosestByWKTHandler> logger, ILineRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<Result<FindClosestLineByWKTResponse>> Handle(FindClosestLineByWKTQuery request, CancellationToken cancellationToken)
    {
        var wkt = WellKnownText.Create(request.wkt   );
        if (wkt.IsFailure) return Result<FindClosestLineByWKTResponse>.Failure(wkt.Error);
        var closest = await _repository.FindClosestLineByWktAsync(wkt.Value, cancellationToken: cancellationToken);
        
        
        _logger.LogInformation("Query Check");
        if (closest.agg == null) return Result<FindClosestLineByWKTResponse>.Failure(RepositoryErrors.QUERY_RETURNS_EMPTY);
        _logger.LogInformation("Query Found Succesfull {closest}",closest.agg);
        var response = FindClosestLineByWKTResponse.FromAgg(closest.agg,closest.distance);
        return Result<FindClosestLineByWKTResponse>.Success(response);
    }
}