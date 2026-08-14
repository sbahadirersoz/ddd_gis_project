using gis.ApplicationLayer.Common;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;
using MediatR;
using Microsoft.Extensions.Logging;

namespace gis.ApplicationLayer.Features.Poi.Queries.FindPoiByName;

public class FindPoiByNameQueryHandler:IRequestHandler<FindPoiByNameQuery,Result<FindPoiByNameQueryResponse>>
{
    private  readonly ILogger<FindPoiByNameQueryHandler> _logger;
    private  readonly IUnitOfWork _unitOfWork;
    private  readonly IPointRepository _pointRepository;
    

    public FindPoiByNameQueryHandler(ILogger<FindPoiByNameQueryHandler> logger, IUnitOfWork unitOfWork, IPointRepository pointRepository)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _pointRepository = pointRepository;
    }

    public async Task<Result<FindPoiByNameQueryResponse>> Handle(FindPoiByNameQuery request, CancellationToken cancellationToken = default)
    {
        var findByEntityExpression = await _pointRepository.FindByEntityExpressionAsync(x=> x.PointName.Equals(request.PointName), cancellationToken: cancellationToken);
        return (findByEntityExpression == null) ?
            Result<FindPoiByNameQueryResponse>.Failure(RepositoryErrors.ENTITY_NOT_FOUND)
            : Result<FindPoiByNameQueryResponse>.Success(FindPoiByNameQueryResponse.CreateFromAgg(findByEntityExpression));
    }
}