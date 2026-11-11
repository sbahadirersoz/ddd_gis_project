using gis.Domain.Entities.IDs;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;

using MediatR;

using Microsoft.Extensions.Logging;

namespace Gis.Application.Features.Line.Queries.Distance.ID.FindMostClosesCountByIdQuery;

public class FindMostClosesCountByIdHandler : IRequestHandler<FindMostClosesCountByIdQuery,
    Result<List<FindMostClosesCountByIdResponse>>>
{
    private readonly ILogger<FindMostClosesCountByIdHandler> _logger;
    private readonly ILineRepository _repo;

    public FindMostClosesCountByIdHandler(ILogger<FindMostClosesCountByIdHandler> logger, ILineRepository repo)
    {
        _logger = logger;
        _repo = repo;
    }

    public async Task<Result<List<FindMostClosesCountByIdResponse>>> Handle(FindMostClosesCountByIdQuery request,
        CancellationToken cancellationToken = default)
    {
        var id = LineID.FromGuid(request.id);
        if (id.IsFailure) return Result<List<FindMostClosesCountByIdResponse>>.Failure(id.Error);
        var entity = await _repo.FindByIdAsync(id.Value, cancellationToken: cancellationToken);
        if (entity == null)
            return Result<List<FindMostClosesCountByIdResponse>>.Failure(RepositoryErrors.ENTITY_NOT_FOUND);
        var list = await _repo.FindMostClosesCountByIdAsync(id.Value, request.count,
            cancellationToken: cancellationToken);
        var response = list.ConvertAll(x => FindMostClosesCountByIdResponse.FromAgg(x.entity, x.distance));
        return Result<List<FindMostClosesCountByIdResponse>>.Success(response);
    }
}