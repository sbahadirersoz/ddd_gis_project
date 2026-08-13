using gis.Domain.Aggregates;
using gis.Domain.Contracts;
using gis.Domain.Entities.Coord;
using gis.Domain.Entities.IDs;
using gis.Domain.Entities.Information;
using gis.Domain.Entities.WKT;
using gis.Domain.HardCodedParameters;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;

namespace gis.Domain.Services;

public class POIDomainService
{
    private readonly IPointRepository _pointRepository;
    private readonly ITopologySuitePointContract _contract;

    public POIDomainService(IPointRepository pointRepository, ITopologySuitePointContract contract)
    {
        _pointRepository = pointRepository;
        _contract = contract;
    }

    #region Update-Create-CreateWithId -Delete

    public async Task<Result<POIAggregate>> UpdatePoiAggregate(POIAggregate poi, Latitude? newLatitude,
        Longitude? newLongitude,
        PointDescription? newPointDesc, PointName? newPointName, POIStatus? newStatus)
    {
        var isLatChanged = newLatitude is not  null && !newLatitude.Equals(poi.Coordinates.Latitude);
        var isLonChanged = newLongitude is not  null && !newLongitude.Equals(poi.Coordinates.Longitude);
        var isDescChanged = newPointDesc is not  null && !newPointDesc.Equals(poi.PointDesc);
        var isNameChanged = newPointName is not  null && !newPointName.Equals(poi.PointName);
        var isStatusChanged = newStatus is not  null && !newStatus.Equals(poi.Status);

        var isAnyChangeOccured = (isDescChanged || isLatChanged || isLonChanged || isNameChanged || isStatusChanged);
        if (!isAnyChangeOccured)
        {
            return Result<POIAggregate>.Failure(DomainServiceErrors.SAME_CREDENTIALS_FOR_UPDATING);
        }

        if (isLatChanged || isLonChanged)
        {
            
            var targetLatitude = newLatitude ??  poi.Coordinates.Latitude  ; 
            var targetLongitude = newLongitude ??  poi.Coordinates. Longitude  ; 
            var changePointCoordinatesAsync = await ChangePointCoordinatesAsync(poi, targetLatitude, targetLongitude);
            if (changePointCoordinatesAsync.IsFailure)
                return Result<POIAggregate>.Failure(changePointCoordinatesAsync.Error);
        }
        if (isNameChanged)
        {
            var changePointName = await ChangePointNameAsync(poi, newPointName!);
            if (changePointName.IsFailure)
                return Result<POIAggregate>.Failure(changePointName.Error);
        }
        if (isDescChanged)
            poi.ChangePointDesc(newPointDesc!);
        if (isStatusChanged)
            poi.ChangeStatus(newStatus!.Value);
        return Result<POIAggregate>.Success(poi);
    }

    public async Task<Result<POIAggregate>> CreatePoiAggregate(Latitude latitude, Longitude longitude,
        PointDescription pointDesc, PointName pointName)
    {
        var methodValidation = await CreateMethodValidation(latitude, longitude, pointDesc, pointName);
        if (methodValidation.IsFailure)
        {
            return Result<POIAggregate>.Failure(methodValidation.Error);
        }

        var coordinateFromLatLon = CreateCoordinateFromLatLon(latitude, longitude);
        if (coordinateFromLatLon.IsFailure)
        {
            return Result<POIAggregate>.Failure(coordinateFromLatLon.Error);
        }

        var result = POIAggregate.create(coordinateFromLatLon.Value, pointDesc, pointName);

        return Result<POIAggregate>.Success(result.Value);
    }

    public async Task<Result<POIAggregate>> CreatePoiAggregateWithId(Latitude latitude, Longitude longitude,
        PointDescription pointDesc, PointName pointName, PointID id)
    {
        var methodValidation = await CreateMethodValidation(latitude, longitude, pointDesc, pointName);
        if (methodValidation.IsFailure)
        {
            return Result<POIAggregate>.Failure(methodValidation.Error);
        }

        var coordinateFromLatLon = CreateCoordinateFromLatLon(latitude, longitude);
        if (coordinateFromLatLon.IsFailure)
        {
            return Result<POIAggregate>.Failure(coordinateFromLatLon.Error);
        }

        var fromPointId = POIAggregate.createFromPointId(coordinateFromLatLon.Value, pointDesc, pointName, id);

        return Result<POIAggregate>.Success(fromPointId.Value);
    }

    public Result SoftDeletePoi(POIAggregate poiAggregate)
    {
        
        ///Burası Business Rules'e göre customise edilebilir  ekstra bir şey eklemedim henüz
        return poiAggregate.SoftDelete();
    }


    #endregion


    #region AggregateFieldChanges

    public async Task<Result> ChangePointCoordinatesAsync(POIAggregate poi, Latitude lat, Longitude lon)
    {
        var validateChangePointCoordinatesAsync = await IsCoordinatesExistsAsync(lat, lon);
        if (validateChangePointCoordinatesAsync.IsFailure)
        {
            return Result.Failure(validateChangePointCoordinatesAsync.Error);
        }

        var coordinateFromLatLon = CreateCoordinateFromLatLon(lat, lon);
        if (coordinateFromLatLon.IsFailure)
        {
            return Result.Failure(coordinateFromLatLon.Error);
        }

        if (coordinateFromLatLon.Value != null) poi.ChangeCoordinates(coordinateFromLatLon.Value);
        return Result.Success();
    }

    public async Task<Result> ChangePointNameAsync(POIAggregate poiAggregate, PointName newPointName)
    {
        var validate = await ValidateChangePointNameAsync(poiAggregate, newPointName);
        if (validate.IsFailure)
        {
            return Result.Failure(validate.Error);
        }

        poiAggregate.ChangePointName(newPointName);
        return Result.Success();
    }

    #endregion

    #region Private   Methods

    private async Task<Result> ValidateChangePointNameAsync(POIAggregate aggregate, PointName pointName)
    {
        if (aggregate.PointName.Equals(pointName))
        {
            return Result.Failure(DomainErrors.POIErrors.PointName.SAME_VALUE_PROVIDED);
        }

        var isPointNameExist = await _pointRepository.IsPointNameExistsAsync(pointName);
        return isPointNameExist ? Result.Failure(RepositoryErrors.VALUE_ALREADY_EXIST_IN_DB) : Result.Success();
    }


    private async Task<Result> IsCoordinatesExistsAsync(Latitude lat, Longitude lon)
    {
        var isCoordinatesExistsAsync = await _pointRepository.IsLatLonCoordinatesExistsAsync(lat, lon);
        return isCoordinatesExistsAsync ? Result.Failure(RepositoryErrors.VALUE_ALREADY_EXIST_IN_DB) : Result.Success();
    }

    private async Task<Result> IsPointNameExists(PointName pointName)
    {
        var isPointNameExistsAsync = await _pointRepository.IsPointNameExistsAsync(pointName);
        return isPointNameExistsAsync ? Result.Failure(RepositoryErrors.VALUE_ALREADY_EXIST_IN_DB) : Result.Success();
    }

    private Result<Coordinates> CreateCoordinateFromLatLon(Latitude latitude, Longitude longitude)
    {
        var wktString = _contract.CreateWktStringFromLatLon(latitude, longitude);
         if (wktString.IsFailure)
            return Result<Coordinates>.Failure(wktString.Error);
        var wktFromContract = WellKnownText.Create(wktString.Value);
        if (wktFromContract.IsFailure)
            return Result<Coordinates>.Failure(wktFromContract.Error);
        var fromLatLon = Coordinates.FromLatLon(latitude, longitude, wktFromContract.Value);
        return fromLatLon.IsFailure
            ? Result<Coordinates>.Failure(fromLatLon.Error)
            : Result<Coordinates>.Success(fromLatLon.Value);
    }

    private async Task<Result> CreateMethodValidation(Latitude latitude, Longitude longitude,
        PointDescription pointDesc, PointName pointName)
    {
        var validateChangePointCoordinatesAsync = await IsCoordinatesExistsAsync(latitude, longitude);
        if (validateChangePointCoordinatesAsync.IsFailure)
        {
            return Result.Failure(validateChangePointCoordinatesAsync.Error);
        }

        if (pointDesc != null && pointDesc.Value.Length > HardCodedPropertities.PointDescriptionPropertities.MaxLength)
        {
            return Result.Failure(DomainErrors.POIErrors.PointDesc.LENGTH_REACHED_MAX_VALUE);
        }


        var isPointNameExists = await IsPointNameExists(pointName);
        if (isPointNameExists.IsFailure)
        {
            return Result.Failure(isPointNameExists.Error);
        }


        return Result.Success();
    }

    #endregion
}