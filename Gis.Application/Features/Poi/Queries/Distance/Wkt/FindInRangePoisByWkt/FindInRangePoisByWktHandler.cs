using gis.Domain.Contracts;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Gis.Application.Features.Poi.Queries.Distance.Wkt.FindInRangePoisByWkt;

public class FindInRangePoisByWktHandler:IRequestHandler<FindInRangePoisByWktQuery,Result<List<FindInRangePoisByWktResponse>>>
{
    private readonly IPointRepository _repository;
    private readonly ILogger<FindInRangePoisByWktHandler> _logger;
    private readonly ITopologySuiteWKTContract _wktContract;

    public FindInRangePoisByWktHandler(ILogger<FindInRangePoisByWktHandler> logger, IPointRepository repository, ITopologySuiteWKTContract wktContract)
    {
        _logger = logger;
        _repository = repository;
        _wktContract = wktContract;
    }

    public async Task<Result<List<FindInRangePoisByWktResponse>>> Handle(FindInRangePoisByWktQuery request, CancellationToken cancellationToken  = default)
    {
        var wkt = _wktContract.ParsePointWkt(request.wkt);
        if (wkt.IsFailure) return  Result<List<FindInRangePoisByWktResponse>>.Failure(wkt.Error);
        
        var findInRangePoisByWktAsync = await _repository.FindInRangePoisByWktAsync(wkt.Value, request.distance, cancellationToken: cancellationToken);
        var result = findInRangePoisByWktAsync.ConvertAll(x=> FindInRangePoisByWktResponse.FromAgg(x.agg,x.distance));
        return Result<List<FindInRangePoisByWktResponse>>.Success(result);
    }
}