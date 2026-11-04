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
    private readonly ITopologySuiteWKTContract _contract;

    public POIDomainService(IPointRepository pointRepository, ITopologySuiteWKTContract contract)
    {
        _pointRepository = pointRepository;
        _contract = contract;
    }

    #region Update-Create-CreateWithId -Delete
    
    

    public async Task<Result<POIAggregate>> UpdatePoiAggregate(POIAggregate poi, Latitude? newLatitude,
        Longitude? newLongitude,
        PointDescription? newPointDesc, PointName? newPointName, POIStatus? newStatus)
    {
        bool hasChanges = 
            (
            newLatitude is not null && !newLatitude.Equals(poi.CoordinateValueObject.Latitude) ||
            newLongitude is not null && !newLongitude.Equals(poi.CoordinateValueObject.Longitude) ||
            newPointDesc is not null && !newPointDesc.Equals(poi.PointDesc) ||
            newPointName is not null && !newPointName.Equals(poi.PointName) ||
            newStatus is not null && !newStatus.Equals(poi.Status)
            );

        if (!hasChanges)
        {
            return Result<POIAggregate>.Failure(DomainServiceErrors.SAME_CREDENTIALS_FOR_UPDATING);
        }

        if (newLatitude != null || newLongitude != null)
        {
            
            var targetLatitude = newLatitude ??  poi.CoordinateValueObject.Latitude  ; 
            var targetLongitude = newLongitude ??  poi.CoordinateValueObject. Longitude  ; 
            var changePointCoordinatesAsync = await ChangePointCoordinatesAsync(poi, targetLatitude, targetLongitude);
            if (changePointCoordinatesAsync.IsFailure)
                return Result<POIAggregate>.Failure(changePointCoordinatesAsync.Error);
        }
        if (newPointName != null)
        {
            var changePointName = await ChangePointNameAsync(poi, newPointName!);
            if (changePointName.IsFailure)
                return Result<POIAggregate>.Failure(changePointName.Error);
        }
        if (newPointDesc != null)
            poi.ChangePointDesc(newPointDesc!);
        
        if (newStatus != null)
            poi.ChangeStatus(newStatus!.Value);
        return Result<POIAggregate>.Success(poi);
    }

    public async Task<Result<POIAggregate>> CreatePoiAggregate(Latitude latitude, Longitude longitude,
        PointDescription pointDesc, PointName pointName)
    {
        var methodValidation = await CreateMethodValidation(latitude, longitude, pointName);
        if (methodValidation.IsFailure)return Result<POIAggregate>.Failure(methodValidation.Error);

        var coordinateFromLatLon = CreateCoordinateFromLonLat(latitude, longitude);
        if (coordinateFromLatLon.IsFailure) return Result<POIAggregate>.Failure(coordinateFromLatLon.Error);

        var result = POIAggregate.create(coordinateFromLatLon.Value, pointDesc, pointName);
        return Result<POIAggregate>.Success(result.Value);
    }

    public async Task<Result<POIAggregate>> CreatePoiAggregateWithId(Latitude latitude, Longitude longitude,
        PointDescription pointDesc, PointName pointName, PointID id)
    {
        var methodValidation = await CreateMethodValidation(latitude, longitude, pointName);
        if (methodValidation.IsFailure) return Result<POIAggregate>.Failure(methodValidation.Error);

        var coordinateFromLatLon = CreateCoordinateFromLonLat(latitude, longitude);
        if (coordinateFromLatLon.IsFailure)  return Result<POIAggregate>.Failure(coordinateFromLatLon.Error);

        var fromPointId = POIAggregate.createFromPointId(coordinateFromLatLon.Value, pointDesc, pointName, id);

        return Result<POIAggregate>.Success(fromPointId.Value);
    }

    public Result SoftDeletePoi(POIAggregate poiAggregate)
    {
        var softDelete = poiAggregate.SoftDelete();
        return (softDelete.IsFailure) ? Result.Failure(softDelete.Error) : Result.Success();
    }


    #endregion


    #region AggregateFieldChanges
    

    public async Task<Result> ChangePointCoordinatesAsync(POIAggregate poi, Latitude lat, Longitude lon)
    {
        var validateChangePointCoordinatesAsync = await IsCoordinatesExistsAsync(lat, lon);
        if (validateChangePointCoordinatesAsync.IsFailure) return Result.Failure(validateChangePointCoordinatesAsync.Error);

        var coordinateFromLatLon = CreateCoordinateFromLonLat(lat, lon);
        if (coordinateFromLatLon.IsFailure) return Result.Failure(coordinateFromLatLon.Error);

         poi.ChangeCoordinates(coordinateFromLatLon.Value);
        return Result.Success();
    }

    public async Task<Result> ChangePointNameAsync(POIAggregate poiAggregate, PointName newPointName)
    {
        var validate = await CheckPointNameExistsAsync( newPointName);
        if (validate.IsFailure) return Result.Failure(validate.Error);

        poiAggregate.ChangePointName(newPointName);
        return Result.Success();
    }

    #endregion

    #region Private   Methods

    private async Task<Result> CheckPointNameExistsAsync( PointName pointName)
    {
        var isPointNameExist = await _pointRepository.IsPointNameExistsAsync(pointName);
        return isPointNameExist ? Result.Failure(RepositoryErrors.VALUE_ALREADY_EXIST_IN_DB) : Result.Success();
    }


    private async Task<Result> IsCoordinatesExistsAsync(Latitude lat, Longitude lon)
    {
        var isCoordinatesExistsAsync = await _pointRepository.IsLonLatCoordinatesExistsAsync(lat, lon);
        return isCoordinatesExistsAsync ? Result.Failure(RepositoryErrors.VALUE_ALREADY_EXIST_IN_DB) : Result.Success();
    }



    private Result<CoordinateValueObject> CreateCoordinateFromLonLat(Latitude latitude, Longitude longitude)
    {
        var wktString = _contract.CreateWktStringFromLonLat(latitude, longitude);
        
         if (wktString.IsFailure)return Result<CoordinateValueObject>.Failure(wktString.Error);
        var wktFromContract = WellKnownText.Create(wktString.Value);
        
        if (wktFromContract.IsFailure) return Result<CoordinateValueObject>.Failure(wktFromContract.Error);
        var fromLatLon = CoordinateValueObject.FromLonLat(latitude, longitude, wktFromContract.Value);
        
        return fromLatLon.IsFailure
            ? Result<CoordinateValueObject>.Failure(fromLatLon.Error)
            : Result<CoordinateValueObject>.Success(fromLatLon.Value);
    }

    private async Task<Result> CreateMethodValidation(Latitude latitude, Longitude longitude, PointName pointName)
    {
        var validateChangePointCoordinatesAsync = await IsCoordinatesExistsAsync(latitude, longitude);
        if (validateChangePointCoordinatesAsync.IsFailure) return Result.Failure(validateChangePointCoordinatesAsync.Error);
        
        var isPointNameExists = await CheckPointNameExistsAsync(pointName);
        if (isPointNameExists.IsFailure) return Result.Failure(isPointNameExists.Error);
        
        return Result.Success();
    }
    #endregion
}