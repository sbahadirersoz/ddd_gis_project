using gis.ApplicationLayer.Common;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;
using MediatR;
using Microsoft.Extensions.Logging;

namespace gis.ApplicationLayer.Features.Poi.Queries.FindPoiById;

public class FindPoiByIdQueryHandler:IRequestHandler<FindPoiByIdQuery,Result<FindPoiByIdQueryResponse>>
{

    private readonly IUnitOfWork _UnitOfWork;
    private readonly ILogger<FindPoiByIdQueryHandler> _logger;

    public FindPoiByIdQueryHandler(IUnitOfWork unitOfWork, ILogger<FindPoiByIdQueryHandler> logger)
    {
        _UnitOfWork = unitOfWork;
        _logger = logger;
    }
    public async Task<Result<FindPoiByIdQueryResponse>> Handle(FindPoiByIdQuery request, CancellationToken cancellationToken =default)
    {
        _logger.LogInformation("Attempting to find poi by id: {id}", request.id);
        var findEntityByIdAsync = await _UnitOfWork.PointRepository.FindEntityByIdAsync(request.id);
        _logger.LogInformation("Validation For Poi Is Exist");
        return (findEntityByIdAsync == null) ?
            Result<FindPoiByIdQueryResponse>.Failure(RepositoryErrors.ENTITY_NOT_FOUND):
            Result<FindPoiByIdQueryResponse>.Success(FindPoiByIdQueryResponse.CreateFromAgg(findEntityByIdAsync));    
    }
}