using gis.Domain.Entities.IDs;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;

using MediatR;

using Microsoft.Extensions.Logging;

namespace Gis.Application.Features.Poi.Queries.FindNearestPoi;

public class FindClosestByIdQueryHandler:IRequestHandler<FindClosestByIdQuery,Result<FindClosestByIdQueryResponse>>
{
    
    private readonly ILogger<FindClosestByIdQueryHandler> _logger;
    private readonly IPointRepository _pointRepository;

    public FindClosestByIdQueryHandler(ILogger<FindClosestByIdQueryHandler> logger, IPointRepository pointRepository)
    {
        _logger = logger;
        _pointRepository = pointRepository;
    }


    public async Task<Result<FindClosestByIdQueryResponse>> Handle(FindClosestByIdQuery request, CancellationToken cancellationToken=default)
    {
        var id = PointID.FromGuid(request.id);
        if (id.IsFailure) return Result<FindClosestByIdQueryResponse>.Failure(id.Error);
        
        var findPoi = await _pointRepository.FindPointByIdAsync(id.Value, cancellationToken: cancellationToken);
        if (findPoi == null)  return Result<FindClosestByIdQueryResponse>.Failure(RepositoryErrors.ENTITY_NOT_FOUND);

        var findClosestSameEntityByGivenIdAsync = await _pointRepository.FindClosestEntityByGivenIdAsync(id.Value, cancellationToken: cancellationToken);
        
        return  (findClosestSameEntityByGivenIdAsync == null)? 
            Result<FindClosestByIdQueryResponse>.Failure(RepositoryErrors.NO_NEIGHBOR_FOUND):
            Result<FindClosestByIdQueryResponse>.Success(FindClosestByIdQueryResponse.FromAggregate(findClosestSameEntityByGivenIdAsync.Value.entity,findClosestSameEntityByGivenIdAsync.Value.distance));
    }
}