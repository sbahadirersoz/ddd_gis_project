using gis.Domain.Aggregates;
using gis.Domain.Contracts;
using gis.Domain.Entities.Coord;
using gis.Domain.Entities.IDs;
using gis.Domain.Entities.Information;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;

namespace gis.Domain.Services;

public class POIDomainService
{
    private readonly IPointRepository _pointRepository;
    private readonly ITopologySuitePointContract _topologyContract;

    public POIDomainService(IPointRepository pointRepository, ITopologySuitePointContract topologyContract)
    {
        _pointRepository = pointRepository;
        _topologyContract = topologyContract;
    }
    public async Task<Result<POIAggregate>> UpdatePoiAggregate(POIAggregate poi, Coordinates? newCoordinates,
        PointDescription? newPointDesc, PointName? newPointName, POIStatus? newStatus)
    {
        
        ///Coordinate Domain Method
        if ( newCoordinates!=null && !poi.Coordinates.Equals(newCoordinates))
        {
            var changePointCoordinatesResult =  await ChangePointCoordinatesAsync(poi, newCoordinates);
            if (changePointCoordinatesResult.IsFailure)
            {
                return Result<POIAggregate>.Failure(changePointCoordinatesResult.Error);
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

    public async Task< Result<POIAggregate>> CreatePOIAggregate(Coordinates coords,
         PointDescription pointDesc, PointName pointName)
    {
        if ( await _pointRepository.IsCoordinatesExistsAsync(coords) ||await _pointRepository.IsPointNameExistsAsync(pointName))
        {
            return Result<POIAggregate>.Failure(RepositoryErrors.VALUE_ALREADY_EXIST_IN_DB);
        }
        var entity = POIAggregate.create(coords, pointDesc, pointName);
        return entity.IsFailure ? Result<POIAggregate>.Failure(entity.Error) : Result<POIAggregate>.Success(entity.Value);
    }
    public async Task< Result<POIAggregate>> CreatePOIAggregateWithID(Coordinates coords,
         PointDescription pointDesc, PointName pointName,PointID id)
    {
        if ( await _pointRepository.IsCoordinatesExistsAsync(coords) ||await _pointRepository.IsPointNameExistsAsync(pointName))
        {
            return Result<POIAggregate>.Failure(RepositoryErrors.VALUE_ALREADY_EXIST_IN_DB);
        }
        var entity = POIAggregate.createFromPointId(coords, pointDesc, pointName,id);
        return entity.IsFailure ? Result<POIAggregate>.Failure(entity.Error) : Result<POIAggregate>.Success(entity.Value);
    }
    public async Task<Result> ChangePointCoordinatesAsync(POIAggregate poi, Coordinates newCoordinates)
    {
        var validateChangePointCoordinatesAsync = await ValidateChangePointCoordinatesAsync(poi,newCoordinates);
        
         if (validateChangePointCoordinatesAsync.IsFailure)
         {
             return Result.Failure(validateChangePointCoordinatesAsync.Error);
         }
         poi.ChangeCoordinates(newCoordinates);
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
    #region Private  Validation Methods 
    private async  Task<Result> ValidateChangePointNameAsync(POIAggregate aggregate, PointName pointName)
    {
        if (aggregate.PointName.Equals(pointName))
        {
            return Result.Failure(DomainErrors.POIErrors.PointNameErrors.SAME_VALUE_PROVIDED);
        }
        var isPointNameExist = await _pointRepository.IsPointNameExistsAsync(pointName);
        return isPointNameExist ? Result.Failure(RepositoryErrors.VALUE_ALREADY_EXIST_IN_DB) : Result.Success();
    }
    
    
    /// <summary>
    /// Buraya  conditionları kararlaştırıp ekleme çıkarma yapılabilir business rules  bittikten sonra tekrar kontrol et
    /// </summary>
    /// <param name="poi"></param>
    /// <param name="newCoordinates"></param>
    /// <returns></returns>
    private async Task<Result> ValidateChangePointCoordinatesAsync(POIAggregate poi, Coordinates newCoordinates)
    {
        var isCoordinatesExistsAsync = await _pointRepository.IsCoordinatesExistsAsync(newCoordinates);
         if (isCoordinatesExistsAsync)
         {
             return Result.Failure(RepositoryErrors.VALUE_ALREADY_EXIST_IN_DB);
         }
         if (_topologyContract.CheckDistanceBetweenPoints(poi.Coordinates, newCoordinates) < HardCodedParameters
                 .HardCodedPropertities.BusinessRuleParameters.MinDistanceBetweenPoints)
         {
             return Result.Failure(BusinessRules.POIRules.DistanceBellowMin);
         }
         return Result.Success();
    }
    #endregion
    
}