using gis.Domain.Aggregates;
using gis.Domain.Entities.IDs;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;

using MediatR;

namespace Gis.Application.Features.Polygon.Queries.FindPolygonById;

public class FindPolygonByIdHandler : IRequestHandler<FindPolygonByIdQuery, Result<FindPolygonByIdResponse>>
{
    private readonly IPolygonRepository _repository;

    public FindPolygonByIdHandler(IPolygonRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<FindPolygonByIdResponse>> Handle(FindPolygonByIdQuery request, CancellationToken cancellationToken = default)
    {
        var polyId = PolygonID.FromGuid(request.id);
        if (polyId.IsFailure) return Result<FindPolygonByIdResponse>.Failure(polyId.Error);
        
        var agg = await _repository.FindByIdAsync(polyId.Value!,false,cancellationToken: cancellationToken);
        if (agg == null) return Result<FindPolygonByIdResponse>.Failure(RepositoryErrors.ENTITY_NOT_FOUND);
        return Result<FindPolygonByIdResponse>.Success(FindPolygonByIdResponse.CreateFromAgg(agg));
    }
}
