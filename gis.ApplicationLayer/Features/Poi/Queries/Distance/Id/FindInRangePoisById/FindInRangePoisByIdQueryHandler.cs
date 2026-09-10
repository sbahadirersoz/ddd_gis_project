using gis.ApplicationLayer.Common;
using gis.ApplicationLayer.Features.Line.Queries.Distance.ID.FindInRangeByIdQuery;
using gis.Domain.Entities.IDs;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;
using MediatR;
using Microsoft.Extensions.Logging;

namespace gis.ApplicationLayer.Features.Poi.Queries.Distance.Id.FindInRangePoisById;


public class FindInRangePoisByIdQueryHandler:IRequestHandler<FindInRangePoisByIdQuery,Result<List<FindInRangePoisByIdQueryResponse>>>
{
    private readonly ILogger<FindInRangePoisByIdQueryHandler> _logger;
    private readonly IPointRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public FindInRangePoisByIdQueryHandler(ILogger<FindInRangePoisByIdQueryHandler> logger, IPointRepository repository, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _repository = repository;
        _unitOfWork = unitOfWork;
    }
        public async Task<Result<List<FindInRangePoisByIdQueryResponse>>> Handle(FindInRangePoisByIdQuery request, CancellationToken cancellationToken)
        {
            
            var fromGuid = PointID.FromGuid(request.id);
            if (fromGuid.IsFailure) return  Result<List<FindInRangePoisByIdQueryResponse>>.Failure(fromGuid.Error);
        
        
            var findInRange = await _repository.FindInRangePoisByIdAsync(fromGuid.Value,request.range, cancellationToken: cancellationToken);
        
            if (findInRange.Count is 0)
                return Result<List<FindInRangePoisByIdQueryResponse>>.Failure(RepositoryErrors.QUERY_RETURNS_EMPTY);
            var response = findInRange.Select(item => FindInRangePoisByIdQueryResponse.FromAgg(item.agg,Math.Round(item.distance,2))).ToList();
            return Result<List<FindInRangePoisByIdQueryResponse>>.Success(response);
        }
}
