using gis.Domain.Entities.Coord;
using gis.Domain.Entities.Information;
using gis.Domain.ResultPattern;

namespace gis.ApplicationLayer.Mapper.PoiAggregate;

public static class PointAggregateVOMapper
{

    public static Result<( Longitude,Latitude)> CreateLonLatFromPrimitive(double latitude, double longitude)
    {
        var latResult = Latitude.Create(latitude);
        var lonResult = Longitude.Create(longitude);
        if (latResult.IsFailure)
        {
            return Result<(Longitude,Latitude)>.Failure(latResult.Error);
        }

        return lonResult.IsFailure ? Result<(Longitude,Latitude)>.Failure(lonResult.Error) : Result<(Longitude,Latitude)>.Success(( lonResult.Value,latResult.Value));
    }

    public static Result<PointName> CreatePointNameFromPrimitives(string pointName)
        => PointName.FromString(pointName);
    public static Result<PointDescription> CreatePointDescFromPrimitives(string pointDesc)
        => PointDescription.FromString(pointDesc);
}