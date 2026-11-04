using gis.Domain.Aggregates;
using gis.Domain.BusinessRules.PolygonRules;
using gis.Domain.BusinessRules.PolygonRules.Abstract;
using gis.Domain.Contracts;
using gis.Domain.Entities.Coord;
using gis.Domain.Entities.Information.Polygon;
using gis.Domain.Entities.Coord.Polygon;
using gis.Domain.Entities.WKT;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;

namespace gis.Domain.Services;

public class PolygonDomainServiceContract : IPolygonDomainServiceContract
{
    private readonly IPolygonRepository _repository;
    private readonly ITopologySuiteWKTContract _wktContract;
    private readonly IPolygonBusinessRulesFactory _validationFactory;

    public PolygonDomainServiceContract(IPolygonRepository repository, ITopologySuiteWKTContract wktContract, IPolygonBusinessRulesFactory validationFactory)
    {
        _repository = repository;
        _wktContract = wktContract;
        _validationFactory = validationFactory;
    }

    public async Task<Result<PolygonAggregate>> CreatePolygonAggregate(PolygonShell shell, PolygonName polygonName, PolygonDescription? polygonDescription)
    {
        var checkName = await IsPolygonNameNotExist(polygonName);
        if (checkName.IsFailure) return Result<PolygonAggregate>.Failure(checkName.Error);

        var wktResult = await CreateWKTFromCoordinates(shell.Coordinates);
        if (wktResult.IsFailure) return Result<PolygonAggregate>.Failure(wktResult.Error);

        var desc = polygonDescription ?? PolygonDescription.CreateFromString(null).Value;
        var createResult = PolygonAggregate.Create(shell, polygonName, desc);
        return createResult;
    }

    public async Task<Result<PolygonAggregate>> UpdatePolygonAggregate(PolygonAggregate poly, PolygonName? polygonName, PolygonDescription? polygonDescription,
        PolygonStatus? status, List<CoordinateValueObject> newHoles)
    {
            bool hasChanges =
            (
                polygonName is not null && !polygonName.Equals(poly.Name) ||
                polygonDescription is not null && !polygonDescription.Equals(poly.Description) ||
                status is not null && !status.Equals(poly.Status) ||
                newHoles is not null && !poly.Holes.Contains(newHoles)
            );

            if (!hasChanges) return Result<PolygonAggregate>.Failure(DomainServiceErrors.SAME_CREDENTIALS_FOR_UPDATING);

            if (polygonName != null)
            {
                var nameValidation = await IsPolygonNameNotExist(polygonName);
                if (nameValidation.IsFailure) return Result<PolygonAggregate>.Failure(nameValidation.Error);
                var updateName = poly.UpdateName(polygonName);
                if (updateName.IsFailure) return Result<PolygonAggregate>.Failure(updateName.Error);
            }

            if (polygonDescription != null)
            {
                poly.UpdateDescription(polygonDescription);
            }

            if (newHoles != null)
            {
                var validateIsHoleInsideTheShell = _validationFactory.ValidateIsHoleInsideTheShell(newHoles,poly);
                var rule =  new PolygonMustBeClosed(newHoles);
                
                var isClosed = rule.Execute();
                if (isClosed.IsFailure) newHoles.Add(newHoles.First());
                if (validateIsHoleInsideTheShell.IsFailure) return Result<PolygonAggregate>.Failure(validateIsHoleInsideTheShell.Error);
                poly.UpdateHoles(newHoles);
            }

            if (status != null)
            {
                var updateStatus = poly.UpdateStatus(status.Value);
                if (updateStatus.IsFailure) return Result<PolygonAggregate>.Failure(updateStatus.Error);
            }

            return Result<PolygonAggregate>.Success(poly);
        }

    public async Task<Result<PolygonAggregate>> AddHoleToPolygon(PolygonAggregate poly, List<CoordinateValueObject> newHole)
    {
        var rule = new PolygonMustBeClosed(newHole);
        
        var isClosed  = rule.Execute();
        if (isClosed.IsFailure) newHole.Add(newHole.First());

        var validateIsHoleInsideTheShell = _validationFactory.ValidateIsHoleInsideTheShell(newHole,poly);
        if (validateIsHoleInsideTheShell.IsFailure) return Result<PolygonAggregate>.Failure(validateIsHoleInsideTheShell.Error);
        
        var addHole = poly.AddHole(newHole);
        return addHole.IsFailure ? Result<PolygonAggregate>.Failure(addHole.Error) : Result<PolygonAggregate>.Success(poly);
    }

    public Result SoftDelete(PolygonAggregate agg)
        {
            var softDelete = agg.SoftDelete();
            return (softDelete.IsFailure) ? Result.Failure(softDelete.Error) : Result.Success();
        }


    #region Private Helpers

    private async Task<Result> IsPolygonNameNotExist(PolygonName name)
    {
        var exists = await _repository.IsPolygonNameExistsAsync(name);
        return !exists ? Result.Success() : Result.Failure(RepositoryErrors.VALUE_ALREADY_EXIST_IN_DB);
    }

    private async Task<Result<WellKnownText>> CreateWKTFromCoordinates(List<CoordinateValueObject> coordinates)
    {
        var wktStringFromCoordList = _wktContract.CreateWktStringFromCoordList(coordinates);
        if (wktStringFromCoordList.IsFailure) return Result<WellKnownText>.Failure(wktStringFromCoordList.Error);
        var wkt = WellKnownText.Create(wktStringFromCoordList.Value);
        return wkt;
    }

    #endregion
}
