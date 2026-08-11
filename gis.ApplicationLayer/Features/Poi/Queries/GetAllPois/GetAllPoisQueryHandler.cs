using gis.ApplicationLayer.Common;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;
using MediatR;
using Microsoft.Extensions.Logging;

namespace gis.ApplicationLayer.Features.Poi.Queries.GetAllPois;

public class GetAllPoisQueryHandler:IRequestHandler<GetAllPoisQuery,Result<IReadOnlyList<GetAllPoisQueryResponse>>>
{
    private readonly ILogger<GetAllPoisQueryHandler> _logger;
    private readonly IUnitOfWork _UnitOfWork;

    public GetAllPoisQueryHandler(IUnitOfWork unitOfWork, ILogger<GetAllPoisQueryHandler> logger)
    {
        _UnitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<IReadOnlyList<GetAllPoisQueryResponse>>> Handle(GetAllPoisQuery request, CancellationToken cancellationToken)
    {
        var poiAggregates = await _UnitOfWork.PointRepository.GetAllEntities();
        if (poiAggregates == null)
            return Result<IReadOnlyList<GetAllPoisQueryResponse>>.Failure(RepositoryErrors.ENTITY_NOT_FOUND);

        IReadOnlyList<GetAllPoisQueryResponse> sources = poiAggregates.Select(x=> GetAllPoisQueryResponse.CreateFromAgg(x)).ToList();
        return Result<IReadOnlyList<GetAllPoisQueryResponse>>.Success(sources);
    }
    
}
