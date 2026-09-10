using gis.Domain.Entities.WKT;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;
using MediatR;
using Microsoft.Extensions.Logging;

namespace gis.ApplicationLayer.Features.Poi.Queries.Distance.Wkt.FindInRangePoisByWkt;

public class FindInRangePoisByWktHandler:IRequestHandler<FindInRangePoisByWktQuery,Result<List<FindInRangePoisByWktResponse>>>
{
    private readonly IPointRepository _repository;
    private readonly ILogger<FindInRangePoisByWktHandler> _logger;

    public FindInRangePoisByWktHandler(ILogger<FindInRangePoisByWktHandler> logger, IPointRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<Result<List<FindInRangePoisByWktResponse>>> Handle(FindInRangePoisByWktQuery request, CancellationToken cancellationToken)
    {
        var wkt = WellKnownText.Create(request.wkt);
        if (wkt.IsFailure) return  Result<List<FindInRangePoisByWktResponse>>.Failure(wkt.Error);
        
        var findInRangePoisByWktAsync = await _repository.FindInRangePoisByWktAsync(wkt.Value, request.distance, cancellationToken: cancellationToken);
        if (findInRangePoisByWktAsync.Count == 0 || findInRangePoisByWktAsync == null) return Result<List<FindInRangePoisByWktResponse>>.Failure(RepositoryErrors.QUERY_RETURNS_EMPTY);
        var result = findInRangePoisByWktAsync.ConvertAll(x=> FindInRangePoisByWktResponse.FromAgg(x.agg,x.distance));
return Result<List<FindInRangePoisByWktResponse>>.Success(result);
    }
}