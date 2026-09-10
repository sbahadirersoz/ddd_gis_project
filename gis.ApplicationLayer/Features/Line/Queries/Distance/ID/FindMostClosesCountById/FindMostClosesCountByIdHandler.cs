using System.Net.Mail;
using gis.Domain.Entities.IDs;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;
using MediatR;
using Microsoft.Extensions.Logging;

namespace gis.ApplicationLayer.Features.Line.Queries.Distance.ID.FindMostClosesCountByIdQuery;

public class FindMostClosesCountByIdHandler:IRequestHandler<FindMostClosesCountByIdQuery,Result<List<FindMostClosesCountByIdResponse>>>
{
    
    private readonly ILogger<FindMostClosesCountByIdHandler> _logger;
    private readonly ILineRepository _repo;

    public FindMostClosesCountByIdHandler(ILogger<FindMostClosesCountByIdHandler> logger, ILineRepository repo)
    {
        _logger = logger;
        _repo = repo;
    }

    public async Task<Result<List<FindMostClosesCountByIdResponse>>> Handle(FindMostClosesCountByIdQuery request, CancellationToken cancellationToken)
    {
        var id = LineID.FromGuid(request.id);
        if (id.IsFailure) return Result<List<FindMostClosesCountByIdResponse>>.Failure(id.Error);
        var list =  await _repo.FindMostClosesCountByIdAsync(id.Value,request.count, cancellationToken: cancellationToken);
        if (list.Count == 0 ) return Result<List<FindMostClosesCountByIdResponse>>.Failure(RepositoryErrors.QUERY_RETURNS_EMPTY);
        var response = list.ConvertAll(x=> FindMostClosesCountByIdResponse.FromAgg(x.entity,x.distance));
        return Result<List<FindMostClosesCountByIdResponse>>.Success(response);
        
    }
}