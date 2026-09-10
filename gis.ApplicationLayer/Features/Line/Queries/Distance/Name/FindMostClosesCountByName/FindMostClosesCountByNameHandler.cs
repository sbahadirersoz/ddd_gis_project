using System.Xml.Schema;
using FluentValidation.Results;
using gis.Domain.Entities.Information.Line;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;
using MediatR;
using Microsoft.Extensions.Logging;

namespace gis.ApplicationLayer.Features.Line.Queries.Distance.Name.FindMostClosesCountByName;

public class FindMostClosesCountByNameHandler:IRequestHandler<FindMostClosesCountByNameQuery,Result<List<FindMostClosesCountByNameResponse>>>
{
    
    private readonly ILogger<FindMostClosesCountByNameHandler> _logger;
    private readonly ILineRepository _lineRepository;

    public FindMostClosesCountByNameHandler(ILogger<FindMostClosesCountByNameHandler> logger, ILineRepository lineRepository)
    {
        _logger = logger;
        _lineRepository = lineRepository;
    }

    public async Task<Result<List<FindMostClosesCountByNameResponse>>> Handle(FindMostClosesCountByNameQuery request, CancellationToken cancellationToken)
    {
        var fromString = LineName.FromString(request.Name);
        if (fromString.IsFailure) return Result<List<FindMostClosesCountByNameResponse>>.Failure(fromString.Error);
        
        var list = await _lineRepository.FindMostClosesCountByLineNameAsync(fromString.Value,request.count, cancellationToken: cancellationToken);
        if (list.Count  == 0) return Result<List<FindMostClosesCountByNameResponse>>.Failure(RepositoryErrors.QUERY_RETURNS_EMPTY);

        var response = list.ConvertAll(x => FindMostClosesCountByNameResponse.FromAgg(x.agg, x.distance));
        return Result<List<FindMostClosesCountByNameResponse>>.Success(response);
    }
}