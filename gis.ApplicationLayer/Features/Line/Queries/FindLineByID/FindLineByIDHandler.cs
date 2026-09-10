using gis.ApplicationLayer.Features.Line.Queries.FindLineByName;
using gis.Domain.Entities.IDs;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;
using MediatR;
using Microsoft.Extensions.Logging;

namespace gis.ApplicationLayer.Features.Line.Queries.FindLineByID;

public class FindLineByIDHandler:IRequestHandler<FindLineByIDQuery,Result< FindLineByIDResponse>>
{
    private readonly  ILogger<FindLineByNameHandler> _logger;
    private readonly  ILineRepository _repository ;
    public async Task<Result<FindLineByIDResponse>> Handle(FindLineByIDQuery request, CancellationToken cancellationToken)
    {
        var fromGuid = LineID.FromGuid(request.id);
        if (fromGuid.IsFailure) return Result<FindLineByIDResponse>.Failure(fromGuid.Error);
        _logger.LogInformation("LineID created Successfully {fromGuid}", fromGuid.Value);
        var entity = await _repository.FindByIdAsync(fromGuid.Value, cancellationToken: cancellationToken);
        if (entity == null) return Result<FindLineByIDResponse>.Failure(RepositoryErrors.QUERY_RETURNS_EMPTY);
        var response = FindLineByIDResponse.FromAgg(entity);
        return Result<FindLineByIDResponse>.Success(response);
    }
}