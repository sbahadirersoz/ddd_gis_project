using gis.Domain.Entities.Information.Line;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;
using MediatR;
using Microsoft.Extensions.Logging;

namespace gis.ApplicationLayer.Features.Line.Queries.Distance.Name.FindInRangeByName;

public class FindInRangeByNameHandler:IRequestHandler<FindInRangeByNameQuery,Result<List<FindInRangeByNameResponse>>>
{
    
    private readonly ILogger<FindInRangeByNameHandler> _logger;
    private readonly ILineRepository _lineRepository;

    public FindInRangeByNameHandler(ILogger<FindInRangeByNameHandler> logger, ILineRepository lineRepository)
    {
        _logger = logger;
        _lineRepository = lineRepository;
    }

    public async Task<Result<List<FindInRangeByNameResponse>>> Handle(FindInRangeByNameQuery request, CancellationToken cancellationToken)
    {
        var name = LineName.FromString(request.name);
        if (name.IsFailure) return Result<List<FindInRangeByNameResponse>>.Failure(name.Error);

        var list = await _lineRepository.FindInRangeLinesByLineNameAsync( name.Value ,request.DistanceInMeter, cancellationToken: cancellationToken);
        if (list.Count  == 0) return Result<List<FindInRangeByNameResponse>>.Failure(RepositoryErrors.QUERY_RETURNS_EMPTY);
        var response = list.ConvertAll(x=>FindInRangeByNameResponse.FromAgg(x.agg,x.distance));
        return Result<List<FindInRangeByNameResponse>>.Success(response);
    }
}