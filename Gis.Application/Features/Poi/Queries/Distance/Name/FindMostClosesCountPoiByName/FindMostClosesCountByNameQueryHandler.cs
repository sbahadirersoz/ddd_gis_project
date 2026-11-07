using gis.Domain.Aggregates;
using gis.Domain.Entities.Information;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Gis.Application.Features.Poi.Queries.Distance.FindNearestCountPoiByName;

public class FindMostClosesCountByNameQueryHandler
    : IRequestHandler<FindMostClosesCountByNameQuery,
        Result<List<FindMostClosesCountByNameQueryResponse>>>
{
    private readonly ILogger<FindMostClosesCountByNameQueryHandler> _logger;
    private readonly IPointRepository _repository;

    public FindMostClosesCountByNameQueryHandler(IPointRepository repository, ILogger<FindMostClosesCountByNameQueryHandler> logger)
    {
        this._repository = repository;
        _logger = logger;
    }

    public async Task<Result<List<FindMostClosesCountByNameQueryResponse>>> Handle(FindMostClosesCountByNameQuery request,
        CancellationToken cancellationToken =default)
    {
        
        var name = PointName.FromString(request.name);
        if (name.IsFailure) return Result<List<FindMostClosesCountByNameQueryResponse>>.Failure(name.Error);

        var entity = await _repository.FindPointByPointNameAsync(name.Value, cancellationToken: cancellationToken);
        if (entity == null)
             return Result<List<FindMostClosesCountByNameQueryResponse>>.Failure(RepositoryErrors.ENTITY_NOT_FOUND);
        var entities = await _repository.FindMostClosesCountByPoiName(
            name.Value, 
            request.count, cancellationToken: cancellationToken);
   
         var result = entities.ConvertAll(x=> FindMostClosesCountByNameQueryResponse.FromAgg(x.agg,x.distance));
         return Result<List<FindMostClosesCountByNameQueryResponse>>.Success(result); }
}