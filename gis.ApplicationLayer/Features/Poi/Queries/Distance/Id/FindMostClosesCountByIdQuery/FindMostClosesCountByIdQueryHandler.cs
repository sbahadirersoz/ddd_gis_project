using gis.ApplicationLayer.Features.Poi.Queries.Distance.Id.FindInRangePoisById;
using gis.Domain.Entities.IDs;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;
using MediatR;
using Microsoft.Extensions.Logging;

namespace gis.ApplicationLayer.Features.Poi.Queries.FindPoisInGivenRange;

public class FindMostClosesCountByIdQueryHandler:IRequestHandler<FindMostClosesCountByIdQuery, Result<List<FindMostClosesCountByIdQueryResponse>>>
{
    
    private readonly ILogger<FindMostClosesCountByIdQueryHandler> _logger;
    private readonly IPointRepository _pointRepository;

    public FindMostClosesCountByIdQueryHandler(ILogger<FindMostClosesCountByIdQueryHandler> logger, IPointRepository pointRepository)
    {
        _logger = logger;
        _pointRepository = pointRepository;
    }

    public async Task<Result<List<FindMostClosesCountByIdQueryResponse>>> Handle(FindMostClosesCountByIdQuery request, CancellationToken cancellationToken =default)
    {
        
        var id = PointID.FromGuid(request.id);
        if (id.IsFailure) return Result<List<FindMostClosesCountByIdQueryResponse>>.Failure(id.Error);
        var queryResponse = await _pointRepository.FindMostClosesCountByIdAsync( id.Value,request.count, cancellationToken: cancellationToken);
        if (queryResponse.Count is 0)
            return Result<List<FindMostClosesCountByIdQueryResponse>>.Failure(RepositoryErrors.QUERY_RETURNS_EMPTY);
        var result
            = queryResponse.Select(x=>FindMostClosesCountByIdQueryResponse.CreateFromAgg(x.entity,Math.Round(x.distance,2) )).ToList();
        return Result<List<FindMostClosesCountByIdQueryResponse>>.Success(result);
    }
}