using gis.ApplicationLayer.Common;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;
using MediatR;
using Microsoft.Extensions.Logging;

namespace gis.ApplicationLayer.Features.Poi.Queries.GetAllPois;

public class GetAllPoisQueryHandler:IRequestHandler<GetAllPoisQuery,Result<IReadOnlyList<GetAllPoisQueryResponse>>>
{
    private readonly ILogger<GetAllPoisQueryHandler> _logger;
    private readonly IPointRepository _pointRepository;

    public GetAllPoisQueryHandler( ILogger<GetAllPoisQueryHandler> logger, IPointRepository pointRepository)
    {
        _logger = logger;
        _pointRepository = pointRepository;
    }

    public async Task<Result<IReadOnlyList<GetAllPoisQueryResponse>>> Handle(GetAllPoisQuery request, CancellationToken cancellationToken = default)
    {
        var poiAggregates = await _pointRepository.GetAllEntitiesAsync(cancellationToken: cancellationToken);
        if (poiAggregates == null)
            return Result<IReadOnlyList<GetAllPoisQueryResponse>>.Failure(RepositoryErrors.ENTITY_NOT_FOUND);

        IReadOnlyList<GetAllPoisQueryResponse> sources = poiAggregates.Select(x=> GetAllPoisQueryResponse.CreateFromAgg(x)).ToList();
        return Result<IReadOnlyList<GetAllPoisQueryResponse>>.Success(sources);
    }
    
}
