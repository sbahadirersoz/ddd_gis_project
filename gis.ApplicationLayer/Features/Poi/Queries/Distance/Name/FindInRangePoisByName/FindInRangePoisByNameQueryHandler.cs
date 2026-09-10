using gis.Domain.Entities.Information;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;
using MediatR;
using Microsoft.Extensions.Logging;

namespace gis.ApplicationLayer.Features.Poi.Queries.Distance.FindInRangePoisByName;

public class FindInRangePoisByNameQueryHandler
    : IRequestHandler<FindInRangePoisByNameQuery, Result<List<FindInRangePoisByNameQueryResponse>>>
{
    private readonly ILogger<FindInRangePoisByNameQueryHandler> _logger;
    private readonly IPointRepository _repository;

    public FindInRangePoisByNameQueryHandler(ILogger<FindInRangePoisByNameQueryHandler> logger, IPointRepository repository) 
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<Result<List<FindInRangePoisByNameQueryResponse>>> Handle(FindInRangePoisByNameQuery request, CancellationToken cancellationToken)
    {
        var fromString = PointName.FromString(request.name);
        if (fromString.IsFailure) return Result<List<FindInRangePoisByNameQueryResponse>>.Failure(fromString.Error);
        
        var isExist = await _repository.IsPointNameExistsAsync(fromString.Value, cancellationToken);
        if (!isExist) return Result<List<FindInRangePoisByNameQueryResponse>>.Failure(RepositoryErrors.ENTITY_NOT_FOUND);
        var findInRangePoisByNameAsync = await _repository.FindInRangePoisByNameAsync(fromString.Value,request.distance, cancellationToken: cancellationToken);
        _logger.LogInformation($"Found {findInRangePoisByNameAsync.Count} pois information");
        if (findInRangePoisByNameAsync == null || findInRangePoisByNameAsync.Count == 0)
            return Result<List<FindInRangePoisByNameQueryResponse>>.Failure(RepositoryErrors.QUERY_RETURNS_EMPTY);
        
        var result = findInRangePoisByNameAsync.Select(x=> FindInRangePoisByNameQueryResponse.FromAgg(x.agg,x.distance)).ToList();
        return Result<List<FindInRangePoisByNameQueryResponse>>.Success(result);
    }
}