using gis.Domain.Aggregates;
using gis.Domain.BusinessRules.LineRules;
using gis.Domain.BusinessRules.LineRules.Coordinate;
using gis.Domain.Contracts;
using gis.Domain.Entities.Coord;
using gis.Domain.Entities.IDs;
using gis.Domain.Entities.Information;
using gis.Domain.Entities.Information.Line;
using gis.Domain.Entities.WKT;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;

namespace gis.Domain.Services;

public class LineDomainServiceContract : ILineDomainServiceContract
{
    private readonly ILineRepository _repository;
    private readonly ITopologySuiteWKTContract _wktContract;

    public LineDomainServiceContract(ILineRepository repository, ITopologySuiteWKTContract wktContract)
    {
        _repository = repository;
        _wktContract = wktContract;
    }

    public async Task<Result<LineAggregate>> CreateLineAggregate(List<CoordinateValueObject> coordinates,
        LineName lineName,
        LineDescription? lineDescription)
    {
        var createRuleset = await CheckCreateRuleset(coordinates, lineName);
        if (createRuleset.IsFailure) return Result<LineAggregate>.Failure(createRuleset.Error);


        var wkt = await CreateWKTFromCoordinates(coordinates);
        if (wkt.IsFailure) return Result<LineAggregate>.Failure(wkt.Error);

        var result = LineAggregate.Create(coordinates, lineName, lineDescription, wkt.Value);
        return result.IsFailure ? Result<LineAggregate>.Failure(result.Error) : Result<LineAggregate>.Success(result.Value);
    }

    public async Task<Result<LineAggregate>> UpdateLineAggregate(LineAggregate agg,
        List<CoordinateValueObject>? coordinates,
        LineName? lineName, LineDescription? lineDescription, LineStatus? status)
    {
        bool hasChanges =
        (
            lineName is not null && !lineName.Equals(agg.LineName) ||
            lineDescription is not null && !lineDescription.Equals(agg.LineDescription) ||
            coordinates is not null && !coordinates.Equals(agg.Coordinates) ||
            status is not null && !status.Equals(agg.LineStatus)
        );
        
        if (!hasChanges) return Result<LineAggregate>.Failure(DomainServiceErrors.SAME_CREDENTIALS_FOR_UPDATING);


        if (lineName != null)
        {
            var nameValidation = await ValidateLineNameDoesNotExistAsync(lineName);
            if (nameValidation.IsFailure) return Result<LineAggregate>.Failure(nameValidation.Error);
            agg.ChangeLineName(lineName);
        }
        
        if (coordinates != null)
        {
            var executeRule = ExecuteRule(new LineShouldHaveAtLeastTwoCoordinates(coordinates));
            if (executeRule.IsFailure)
                return Result<LineAggregate>.Failure(executeRule.Error);
            
            var wktFromCoordinates = await CreateWKTFromCoordinates(coordinates);
            if (wktFromCoordinates.IsFailure) return Result<LineAggregate>.Failure(wktFromCoordinates.Error);
            
            agg.ChangeCoordinates(coordinates,wktFromCoordinates.Value);
        }

        if (status != null)
        {
            agg.ChangeStatus(status.Value);
        }
        return Result<LineAggregate>.Success(agg);
    }

    public Result SoftDelete(LineAggregate agg)
    {
        var softDelete = agg.SoftDelete();
        return (softDelete.IsFailure) ? Result.Failure(softDelete.Error) : Result.Success();
    }

    #region Private Helper  Methods

    private async Task<Result> CheckCreateRuleset(List<CoordinateValueObject> coordinates, LineName lineName)
    {
        var isLineNameNotExists = await IsLineNameNotExist(lineName);
        if (isLineNameNotExists.IsFailure) return Result.Failure(isLineNameNotExists.Error);

        var isCoordsValid = ExecuteRule(new LineShouldHaveAtLeastTwoCoordinates(coordinates));
        if (isCoordsValid.IsFailure) return Result<LineAggregate>.Failure(isCoordsValid.Error);

        return Result.Success();
    }


    private async Task<Result> CheckLineExistById(LineID id)
    {
        var findByIdAsync = await _repository.FindByIdAsync(id);
        return (findByIdAsync == null) ? Result.Failure(RepositoryErrors.ENTITY_NOT_FOUND) : Result.Success();
    }

    private async Task<Result<LineAggregate>> FindLineById(LineID id)
    {
        var findByIdAsync = await _repository.FindByIdAsync(id);
        return (findByIdAsync != null)
            ? Result<LineAggregate>.Success(findByIdAsync)
            : Result<LineAggregate>.Failure(RepositoryErrors.ENTITY_NOT_FOUND);
    }

    private async Task<Result> IsLineNameNotExist(LineName name)
    {
        var existsAsync = await _repository.IsLineNameExistsAsync(name);
        return !existsAsync ? Result.Success() : Result.Failure(RepositoryErrors.VALUE_ALREADY_EXIST_IN_DB);
    }

    private async Task<Result> ValidateLineNameDoesNotExistAsync(LineName name)
    {
        var existsAsync = await _repository.IsLineNameExistsAsync(name);
        return existsAsync ? Result.Failure(RepositoryErrors.VALUE_ALREADY_EXIST_IN_DB) : Result.Success();
    }

    private async Task< Result<WellKnownText>> CreateWKTFromCoordinates(List<CoordinateValueObject> coordinates)
    {
        var wktStringFromCoordList = _wktContract.CreateWktStringFromCoordList(coordinates);
        if (wktStringFromCoordList.IsFailure) return Result<WellKnownText>.Failure(wktStringFromCoordList.Error);
        var wkt = WellKnownText.Create(wktStringFromCoordList.Value);
        if (wkt.IsFailure) return Result<WellKnownText>.Failure(wkt.Error);
        return Result<WellKnownText>.Success(wkt.Value);
    }

    private Result ExecuteRule(ILineRule rule)
    {
        var execute = rule.Execute();
        return (execute.IsFailure) ? Result.Failure(execute.Error) : Result.Success();
    }
    #endregion
}