using gis.Domain.Aggregates;
using gis.Domain.Contracts;
using gis.Domain.Entities.Coord;
using gis.Domain.Entities.IDs;
using gis.Domain.Entities.Information;
using gis.Domain.Entities.WKT;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;

namespace gis.Domain.Services;

public class POIDomainService
{
    private readonly IPointRepository _pointRepository;
    private readonly ITopologySuitePointContract _contract;

    public POIDomainService(IPointRepository pointRepository)
    {
        _pointRepository = pointRepository;
    }
    public async Task<Result<POIAggregate>> UpdatePoiAggregate(POIAggregate poi, Latitude? latitude,Longitude? longitude, 
        PointDescription? newPointDesc, PointName? newPointName, POIStatus? newStatus)
    {
        
        ///Coordinate Domain Method

        if (latitude != null || longitude != null)
        {

            if (!poi.Coordinates.Latitude.Equals(latitude) || !poi.Coordinates.Longitude.Equals(longitude))
            {
                var changePointCoordinatesResult = await ChangePointCoordinatesAsync(poi, latitude, longitude);
                if (changePointCoordinatesResult.IsFailure)
                {
                    return Result<POIAggregate>.Failure(changePointCoordinatesResult.Error);
                }
            }
        }

        ///PointName Domain Method
        if (newPointName!= null && !poi.PointName.Equals(newPointName))
        {
            var changePointNameAsync = await ChangePointNameAsync(poi, newPointName);
            if (changePointNameAsync.IsFailure)
            {
                return Result<POIAggregate>.Failure(changePointNameAsync.Error);
            }
        }
        /// Unique ya da db den kontrol edilmesi gereken bir yapı olmadığı için direkt domainin içindeki methodu  işleyebiliriz
        if (!object.Equals(poi.PointDesc, newPointDesc)&& newPointDesc != null)
        {
            poi.ChangePointDesc(newPointDesc);
        }
        if (newStatus != null && !poi.Status.Equals(newStatus))
        {
            poi.ChangeStatus(newStatus.Value);
        }
        
        return Result<POIAggregate>.Success(poi);
        
    }

    public async Task< Result<POIAggregate>> CreatePoiAggregate(Latitude latitude, Longitude longitude,
         PointDescription pointDesc, PointName pointName)
    {
        var methodValidation =  await CreateMethodValidation(latitude, longitude, pointDesc, pointName);
        if (methodValidation.IsFailure)
        {
            return Result<POIAggregate>.Failure(methodValidation.Error);
        }
        var coordinateFromLatLon = CreateCoordinateFromLatLon(latitude, longitude);
        if (coordinateFromLatLon.IsFailure)
        {
            return Result<POIAggregate>.Failure(coordinateFromLatLon.Error);
        }

        var result = POIAggregate.create(coordinateFromLatLon.Value,pointDesc,pointName);

        return Result<POIAggregate>.Success( result.Value);
        
    }
    public async Task< Result<POIAggregate>> CreatePoiAggregateWithId(Latitude latitude, Longitude longitude,
         PointDescription pointDesc, PointName pointName,PointID id)
    {
        var methodValidation =  await CreateMethodValidation(latitude, longitude, pointDesc, pointName);
        if (methodValidation.IsFailure)
        {
            return Result<POIAggregate>.Failure(methodValidation.Error);
        }
        var coordinateFromLatLon = CreateCoordinateFromLatLon(latitude, longitude);
        if (coordinateFromLatLon.IsFailure)
        {
            return Result<POIAggregate>.Failure(coordinateFromLatLon.Error);
        }

        var fromPointId = POIAggregate.createFromPointId(coordinateFromLatLon.Value,pointDesc,pointName,id);

        return Result<POIAggregate>.Success( fromPointId.Value);
    }
    public async Task<Result> ChangePointCoordinatesAsync(POIAggregate poi, Latitude lat,  Longitude lon)
    {
        var validateChangePointCoordinatesAsync = await IsCoordinatesExistsAsync(lat,lon);
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
            return  Result.Failure(validate.Error);
        }
        poiAggregate.ChangePointName(newPointName);
        return Result.Success();
    }
    #region Private   Methods 
    private async  Task<Result> ValidateChangePointNameAsync(POIAggregate aggregate, PointName pointName)
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
        var isCoordinatesExistsAsync = await _pointRepository.IsLatLonCoordinatesExistsAsync(lat,lon);
        return isCoordinatesExistsAsync ? 
            Result.Failure(RepositoryErrors.VALUE_ALREADY_EXIST_IN_DB): Result.Success();
    }    private async Task<Result> IsPointNameExists(PointName pointName  )
    {
        var isPointNameExistsAsync = await _pointRepository.IsPointNameExistsAsync(pointName);
        return isPointNameExistsAsync ? 
            Result.Failure(RepositoryErrors.VALUE_ALREADY_EXIST_IN_DB): Result.Success();
    }

    private Result<Coordinates> CreateCoordinateFromLatLon(Latitude latitude, Longitude longitude)
    {
        var wktString = _contract.CreateWktStringFromLatLon(latitude,longitude);
        var wktFromContract = WellKnownText.Create(wktString);
        if (wktFromContract.IsFailure)
        {
            return Result<Coordinates>.Failure(wktFromContract.Error);
        }
        var fromLatLon = Coordinates.FromLatLon(latitude, longitude, wktFromContract.Value);
        return fromLatLon.IsFailure ? Result<Coordinates>.Failure(fromLatLon.Error) : Result<Coordinates>.Success(fromLatLon.Value);
    }

    private async Task<Result> CreateMethodValidation(Latitude latitude, Longitude longitude,
        PointDescription pointDesc, PointName pointName)
    {
        var validateChangePointCoordinatesAsync = await IsCoordinatesExistsAsync(latitude,longitude);
        if (validateChangePointCoordinatesAsync.IsFailure)
        {
            return Result.Failure(validateChangePointCoordinatesAsync.Error);
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