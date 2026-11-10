using gis.Domain.Entities.IDs;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Gis.Application.Features.Line.Queries.Distance.ID.FindInRangeByIdQuery;

public class FindInRangeLinesByIdHandler:IRequestHandler<FindInRangeLinesByIdQuery,Result<List<FindInRangeLinesByIdResponse>>>
{
    private readonly  ILogger<FindInRangeLinesByIdHandler> _logger;
    private readonly  ILineRepository _repo;

    public FindInRangeLinesByIdHandler(ILogger<FindInRangeLinesByIdHandler> logger, ILineRepository repo)
    {
        _logger = logger;
        _repo = repo;
    }

    public async Task<Result<List<FindInRangeLinesByIdResponse>>> Handle(FindInRangeLinesByIdQuery request, CancellationToken cancellationToken  = default)
    {
        var fromGuid = LineID.FromGuid(request.id);
        if (fromGuid.IsFailure) return Result<List<FindInRangeLinesByIdResponse>>.Failure(fromGuid.Error);
        var entity = await _repo.FindByIdAsync(fromGuid.Value, cancellationToken: cancellationToken);
        if (entity == null) return Result<List<FindInRangeLinesByIdResponse>>.Failure(RepositoryErrors.ENTITY_NOT_FOUND);
        var list = await _repo.FindNearbySameEntityByGivenIdAsync(fromGuid.Value,request.distance, cancellationToken: cancellationToken);
        var response = list.ConvertAll(x => FindInRangeLinesByIdResponse.FromAgg(x.entity, x.distance));
        return Result<List<FindInRangeLinesByIdResponse>>.Success(response);
    }

}