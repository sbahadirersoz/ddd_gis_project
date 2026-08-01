using gis.Domain.Entities.WKT;
using gis.Domain.HardCodedParameters;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;

namespace gis.Domain.Entities.Coord;
/// Domain Info
/// Longitude X Latitude Y
/// 

public record Coordinates
{
    public Latitude Latitude { get; }
    public Longitude Longitude { get;  }
    public double? Altitude { get;  }
    public WellKnownText WKT { get; private set; }
    private Coordinates(Latitude latitude, Longitude longitude, double? altitude, WellKnownText wkt)
    {
        Latitude = latitude;
        Longitude = longitude;
        WKT = wkt;
        Altitude  = altitude ?? null;
    }
    public static Result<Coordinates> FromLatLon(Latitude latitude, Longitude longitude,WellKnownText wkt)
    {
        return Result<Coordinates>.Success(new Coordinates(latitude, longitude  , null,wkt));
    }
    public static Result<Coordinates> FromLatLonAlt(Latitude latitude, Longitude longitude, double altitude , WellKnownText wkt)
    {
        var validation = AltitudeValidation(altitude);
        return validation.IsFailure ? Result<Coordinates>.Failure(validation.Error) : Result<Coordinates>.Success(new Coordinates(latitude, longitude, altitude,wkt));
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