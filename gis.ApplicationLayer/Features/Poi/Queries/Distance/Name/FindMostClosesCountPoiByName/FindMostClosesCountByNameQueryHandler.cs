using gis.Domain.Aggregates;
using gis.Domain.Entities.Information;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;
using MediatR;
using Microsoft.Extensions.Logging;

namespace gis.ApplicationLayer.Features.Poi.Queries.Distance.FindNearestCountPoiByName;

public class FindMostClosesCountByNameQueryHandler
    : IRequestHandler<FindMostClosesCountByNameQuery,
        Result<List<FindMostClosesCountByNameQueryResponse>>>
{
    private readonly ILogger<FindMostClosesCountByNameQueryHandler> _logger;
    private readonly IPointRepository pointRepository;

    public FindMostClosesCountByNameQueryHandler(IPointRepository pointRepository, ILogger<FindMostClosesCountByNameQueryHandler> logger)
    {
        this.pointRepository = pointRepository;
        _logger = logger;
    }

    public async Task<Result<List<FindMostClosesCountByNameQueryResponse>>> Handle(FindMostClosesCountByNameQuery request,
        CancellationToken cancellationToken =default)
    {
        
        var name = PointName.FromString(request.name);
        if (name.IsFailure) return Result<List<FindMostClosesCountByNameQueryResponse>>.Failure(name.Error);
        var entities = await pointRepository.FindMostClosesCountByPoiName(
            name.Value, 
            request.count, cancellationToken: cancellationToken);

         if (entities is null || entities.Count == 0)
             return Result<List<FindMostClosesCountByNameQueryResponse>>.Failure(RepositoryErrors
                 .QUERY_RETURNS_EMPTY);

         var result = entities.ConvertAll(x=> FindMostClosesCountByNameQueryResponse.FromAgg(x.agg,x.distance));
         return Result<List<FindMostClosesCountByNameQueryResponse>>.Success(result); }
}