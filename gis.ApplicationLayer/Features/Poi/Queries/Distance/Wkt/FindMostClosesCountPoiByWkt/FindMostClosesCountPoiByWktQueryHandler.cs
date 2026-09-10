using gis.Domain.Entities.WKT;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;
using MediatR;
using Microsoft.Extensions.Logging;

namespace gis.ApplicationLayer.Features.Poi.Queries.Distance.Wkt.FindMostClosesCountPoiByWkt;

public class FindMostClosesCountPoiByWktQueryHandler:IRequestHandler<FindMostClosesCountPoiByWktQuery,Result<List<FindMostClosesCountPoiByWktQueryResponse>>>
{
    private readonly IPointRepository _repository;
    private readonly ILogger<FindMostClosesCountPoiByWktQueryHandler> _logger;

    public FindMostClosesCountPoiByWktQueryHandler(IPointRepository repository, ILogger<FindMostClosesCountPoiByWktQueryHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<List<FindMostClosesCountPoiByWktQueryResponse>>> Handle(FindMostClosesCountPoiByWktQuery request, CancellationToken cancellationToken)
    {
        var wkt = WellKnownText.Create(request.wkt);
        if (wkt.IsFailure) return Result<List<FindMostClosesCountPoiByWktQueryResponse>>.Failure(wkt.Error);
        

        var findByWktAsync =  await _repository.FindByWktAsync(wkt.Value, cancellationToken: cancellationToken);
        if (findByWktAsync == null) return Result<List<FindMostClosesCountPoiByWktQueryResponse>>.Failure(RepositoryErrors.QUERY_RETURNS_EMPTY);
        var result = await _repository.FindMostClosesCountByWktAsync(wkt.Value,request.count, cancellationToken: cancellationToken);
        var response = result.Select(x=> FindMostClosesCountPoiByWktQueryResponse.FromAgg(x.agg,x.distance)).ToList();
        return Result<List<FindMostClosesCountPoiByWktQueryResponse>>.Success(response);
    }
}