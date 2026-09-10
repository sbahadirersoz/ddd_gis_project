using gis.Domain.Entities.Information.Line;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;
using MediatR;
using Microsoft.Extensions.Logging;

namespace gis.ApplicationLayer.Features.Line.Queries.Distance.Name.FindClosestByName;

public class FindClosestLineByNameHandler:IRequestHandler<FindClosestLineByNameQuery,Result<FindClosestLineByNameResponse>>
{
        
    private readonly ILogger<FindClosestLineByNameHandler> _logger;
    private readonly ILineRepository _lineRepository;

    public FindClosestLineByNameHandler(ILogger<FindClosestLineByNameHandler> logger, ILineRepository lineRepository)
    {
        _logger = logger;
        _lineRepository = lineRepository;
    }

    public async Task<Result<FindClosestLineByNameResponse>> Handle(FindClosestLineByNameQuery request, CancellationToken cancellationToken)
    {

        var name = LineName.FromString(request.name);
        if (name.IsFailure) return Result<FindClosestLineByNameResponse>.Failure(name.Error);
        
        var result = await _lineRepository.FindClosestLineByNameAsync(name.Value, cancellationToken: cancellationToken);
        if (result.agg == null) return Result<FindClosestLineByNameResponse>.Failure(RepositoryErrors.QUERY_RETURNS_EMPTY);
        
        _logger.LogDebug(" result found  {result}",result);
        var response = FindClosestLineByNameResponse.FromAgg(result.agg,result.distance);
        return Result<FindClosestLineByNameResponse>.Success(response);
    }
}