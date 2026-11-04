using gis.Domain.Entities.WKT;
using gis.Domain.HardCodedParameters;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;

namespace gis.Domain.Entities.Coord;
/// Domain Info
/// Longitude X Latitude Y
/// 

public record CoordinateValueObject
{
    public Longitude Longitude { get;  }
    public Latitude Latitude { get; }
    public double? Altitude { get;  }
    public WellKnownText WKT { get; private set; }
    private CoordinateValueObject( Longitude longitude,Latitude latitude, double? altitude, WellKnownText wkt)
    {
        Latitude = latitude;
        Longitude = longitude;
        WKT = wkt;
        Altitude  = altitude ?? null;
    }
    public static Result<CoordinateValueObject> FromLonLat(Latitude latitude, Longitude longitude,WellKnownText wkt)
    {
        return Result<CoordinateValueObject>.Success(new CoordinateValueObject( longitude  ,latitude, null,wkt));
    }   public static Result<CoordinateValueObject> ForLineCoordCreation(Latitude latitude, Longitude longitude)
    {
        return Result<CoordinateValueObject>.Success(new CoordinateValueObject( longitude  ,latitude, null,null));
    }   public static Result<CoordinateValueObject> ForPolygonCoordCreation(Latitude latitude, Longitude longitude)
    {
        return Result<CoordinateValueObject>.Success(new CoordinateValueObject( longitude  ,latitude, null,null));
    }
    public static Result<CoordinateValueObject> FromLonLatAlt(Latitude latitude, Longitude longitude, double altitude , WellKnownText wkt)
    {
        var validation = AltitudeValidation(altitude);
        return validation.IsFailure ? Result<CoordinateValueObject>.Failure(validation.Error) : Result<CoordinateValueObject>.Success(new CoordinateValueObject( longitude,latitude, altitude,wkt));
    }

    private static Result AltitudeValidation(double altitude)
    {
        return altitude switch
        {
            < HardCodedPropertities.CoordinatePropertities.MinAltitude 
                => Result.Failure(DomainErrors.POIErrors.Coordinate.ALTITUDE_COORDINATE_IS_UNDER_MIN_VALUE_ERROR),
            > HardCodedPropertities.CoordinatePropertities.MaxAltitude 
                => Result.Failure(DomainErrors.POIErrors.Coordinate.LONGITUDE_COORDINATE_IS_OVER_MAX_VALUE_ERROR),
            _ =>
                Result.Success()
        };
    }
}
