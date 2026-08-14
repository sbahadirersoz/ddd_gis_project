using gis.ApplicationLayer.Common;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;
using MediatR;
using Microsoft.Extensions.Logging;

namespace gis.ApplicationLayer.Features.Poi.Queries.FindPoiByName;

public class FindPoiByNameQueryHandler:IRequestHandler<FindPoiByNameQuery,Result<FindPoiByNameQueryResponse>>
{
    private  readonly ILogger<FindPoiByNameQueryHandler> _logger;
    private  readonly IUnitOfWork _unitOfWork;

    public FindPoiByNameQueryHandler(ILogger<FindPoiByNameQueryHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<FindPoiByNameQueryResponse>> Handle(FindPoiByNameQuery request, CancellationToken cancellationToken = default)
    {
        var findByEntityExpression = await _unitOfWork.PointRepository.FindByEntityAsyncExpression(x=> x.PointName.Equals(request.PointName));
        return (findByEntityExpression == null) ?
            Result<FindPoiByNameQueryResponse>.Failure(RepositoryErrors.ENTITY_NOT_FOUND)
            : Result<FindPoiByNameQueryResponse>.Success(FindPoiByNameQueryResponse.CreateFromAgg(findByEntityExpression));
    }
}