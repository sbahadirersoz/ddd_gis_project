using gis.Domain.HardCodedParameters;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;

namespace gis.Domain.Entities.Coord;
/// Domain Info
/// Longitude X Latitude Y
/// 

public record Coordinates
{
    public double Latitude { get; init; }
    public double Longitude { get; init; }
    public double? Altitude { get; init; }

    private Coordinates(double latitude, double longitude, double? altitude)
    {
        Latitude = latitude;
        Longitude = longitude;
        Altitude  = altitude ?? null;
    }
    public static Result<Coordinates> FromLatLon(double latitude, double longitude)
    {
        var validation = validateCoords(latitude, longitude ,null);
        return validation.IsFailure ? Result<Coordinates>.Failure(validation.Error) : Result<Coordinates>.Success(new Coordinates(latitude, longitude  , null));
    }
    public static Result<Coordinates> FromLatLonAlt(double latitude, double longitude, double altitude)
    {
        var validation = validateCoords(latitude, longitude, altitude);
        return validation.IsFailure ? Result<Coordinates>.Failure(validation.Error) : Result<Coordinates>.Success(new Coordinates(latitude, longitude, altitude));
    }
    
    
    private static Result validateCoords(double latitude, double longitude, double? altitude)
    {
        var latVal = latitudeValidation(latitude);
        var longVal = longitudeValidation(longitude);
        if (latVal.IsFailure)
        {
            return Result.Failure(latVal.Error);
        }
        if (longVal.IsFailure)
        {
            return Result.Failure(longVal.Error);
        }

        if (!altitude.HasValue) return Result.Success();
        var validation = altitudeValidation(altitude.Value);
        return validation.IsFailure ? Result.Failure(validation.Error) : Result.Success();
    }

    private static Result latitudeValidation(double latitude)
    {
        return latitude switch
        {
            < HardCodedPropertities.CoordinatePropertities.MinLatitude => Result.Failure(DomainErrors.POIErrors.CoordinateErrors.LATITUDE_COORDINATE_IS_UNDER_MIN_VALUE_ERROR),
            > HardCodedPropertities.CoordinatePropertities.MaxLatitude => Result.Failure(DomainErrors.POIErrors.CoordinateErrors.LATITUDE_COORDINATE_IS_OVER_MAX_VALUE_ERROR),
            _ => Result.Success()
        };
    }
    private static Result longitudeValidation(double longitude)
    {
        return longitude switch
        {
            < HardCodedPropertities.CoordinatePropertities.MinLongitude
                => Result.Failure(DomainErrors
                .POIErrors.CoordinateErrors.LONGITUDE_COORDINATE_IS_UNDER_MIN_VALUE_ERROR),
            > HardCodedPropertities.CoordinatePropertities.MaxLongitude 
                => Result.Failure(DomainErrors
                .POIErrors.CoordinateErrors.LONGITUDE_COORDINATE_IS_OVER_MAX_VALUE_ERROR),
            _ => Result.Success()
        };
    }private static Result altitudeValidation(double altitude)
    {
        return altitude switch
        {
            < HardCodedPropertities.CoordinatePropertities.MinAltitude 
                => Result.Failure(DomainErrors.POIErrors.CoordinateErrors.ALTITUDE_COORDINATE_IS_UNDER_MIN_VALUE_ERROR),
            > HardCodedPropertities.CoordinatePropertities.MaxAltitude 
                => Result.Failure(DomainErrors.POIErrors.CoordinateErrors.LONGITUDE_COORDINATE_IS_OVER_MAX_VALUE_ERROR),
            _ =>
                Result.Success()
        };
    }
}