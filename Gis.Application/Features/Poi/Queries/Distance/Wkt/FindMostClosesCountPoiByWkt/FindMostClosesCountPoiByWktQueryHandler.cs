using gis.Domain.Contracts;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Gis.Application.Features.Poi.Queries.Distance.Wkt.FindMostClosesCountPoiByWkt;

public class FindMostClosesCountPoiByWktQueryHandler:IRequestHandler<FindMostClosesCountPoiByWktQuery,Result<List<FindMostClosesCountPoiByWktQueryResponse>>>
{
    private readonly IPointRepository _repository;
    private readonly ILogger<FindMostClosesCountPoiByWktQueryHandler> _logger;
    private readonly ITopologySuiteWKTContract _wktContract;

    public FindMostClosesCountPoiByWktQueryHandler(IPointRepository repository, ILogger<FindMostClosesCountPoiByWktQueryHandler> logger, ITopologySuiteWKTContract wktContract)
    {
        _repository = repository;
        _logger = logger;
        _wktContract = wktContract;
    }

    public async Task<Result<List<FindMostClosesCountPoiByWktQueryResponse>>> Handle(FindMostClosesCountPoiByWktQuery request, CancellationToken cancellationToken)
    {
        var wkt = _wktContract.ParsePointWkt(request.wkt);
        if (wkt.IsFailure) return Result<List<FindMostClosesCountPoiByWktQueryResponse>>.Failure(wkt.Error);
        

        var result = await _repository.FindMostClosesCountByWktAsync(wkt.Value,request.count, cancellationToken: cancellationToken);
        var response = result.Select(x=> FindMostClosesCountPoiByWktQueryResponse.FromAgg(x.agg,x.distance)).ToList();
        return Result<List<FindMostClosesCountPoiByWktQueryResponse>>.Success(response);
    }
}