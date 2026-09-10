using gis.Domain.Entities.Information.Line;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;
using MediatR;
using Microsoft.Extensions.Logging;

namespace gis.ApplicationLayer.Features.Line.Queries.FindLineByName;

public class FindLineByNameHandler:IRequestHandler<FindLineByNameQuery,Result<FindLineByNameResponse>>
{
    private readonly  ILogger<FindLineByNameHandler> _logger;
    private readonly  ILineRepository _repository;
    public async Task<Result<FindLineByNameResponse>> Handle(FindLineByNameQuery request, CancellationToken cancellationToken)
    {
        var name = LineName.FromString(request.name);
        if (name.IsFailure) return Result<FindLineByNameResponse>.Failure(name.Error);
        _logger.LogInformation("Name Created Successfully {name}", name);
        var entity = await _repository.FindByNameAsync(name.Value, cancellationToken: cancellationToken);
        if (entity == null) return Result<FindLineByNameResponse>.Failure(RepositoryErrors.QUERY_RETURNS_EMPTY);

        var response = FindLineByNameResponse.FromAgg(entity);
        return Result<FindLineByNameResponse>.Success(response);
    }
}