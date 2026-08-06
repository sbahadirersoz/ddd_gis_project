using gis.Domain.Contracts;
using gis.Domain.Entities.Coord;
using gis.Domain.Entities.Information;
using gis.Domain.ResultPattern;

namespace gis.ApplicationLayer.Mapper.PoiAggregate;

public static class PointAggregateVOMapper
{

    public static Result<(Latitude, Longitude)> CreateLatLonFromPrimitives(double latitude, double longitude)
    {
        var latResult = Latitude.Create(latitude);
        var lonResult = Longitude.Create(longitude);
        if (latResult.IsFailure)
        {
            return Result<(Latitude, Longitude)>.Failure(latResult.Error);
        }

        if (lonResult.IsFailure)
        {
            return Result<(Latitude, Longitude)>.Failure(lonResult.Error);
        }

        return Result<(Latitude, Longitude)>.Success((latResult.Value, lonResult.Value));
    }

    public static Result<PointName> CreatePointNameFromPrimitives(string pointName)
        => PointName.FromString(pointName).IsSuccess
            ? Result<PointName>.Success(PointName.FromString(pointName).Value)
            : Result<PointName>.Failure(PointName.FromString(pointName).Error);
    
    public static Result<PointDescription> CreatePointDescFromPrimitives(string pointDesc)
        => PointDescription.FromString(pointDesc).IsSuccess
            ? Result<PointDescription>.Success(PointDescription.FromString(pointDesc).Value)
            : Result<PointDescription>.Failure(PointDescription.FromString(pointDesc).Error);
}