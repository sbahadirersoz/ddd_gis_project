using gis.Domain.Entities.IDs;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;
using MediatR;
using Microsoft.Extensions.Logging;

namespace gis.ApplicationLayer.Features.Line.Queries.Distance.ID.FindClosestByIdQuery;

public class FindClosestLineByIdHandler:IRequestHandler<FindClosestLineByIdQuery,Result<FindClosestLineByIdResponse>>
{
    private readonly  ILogger<FindClosestLineByIdHandler> _logger;
    private readonly  ILineRepository _repo;

    public FindClosestLineByIdHandler(ILogger<FindClosestLineByIdHandler> logger, ILineRepository repo)
    {
        _logger = logger;
        _repo = repo;
    }

    public async Task<Result<FindClosestLineByIdResponse>> Handle(FindClosestLineByIdQuery request, CancellationToken cancellationToken)
    {
        var fromGuid = LineID.FromGuid(request.id);
        if (fromGuid.IsFailure) return Result<FindClosestLineByIdResponse>.Failure(fromGuid.Error);
        var entity = await _repo.FindClosestEntityByGivenIdAsync(fromGuid.Value, cancellationToken: cancellationToken);
        if (entity == null) return Result<FindClosestLineByIdResponse>.Failure(RepositoryErrors.QUERY_RETURNS_EMPTY);
        var response  = FindClosestLineByIdResponse.FromAgg(entity.Value.entity,entity.Value.distance);
        return Result<FindClosestLineByIdResponse>.Success(response);
    }
}