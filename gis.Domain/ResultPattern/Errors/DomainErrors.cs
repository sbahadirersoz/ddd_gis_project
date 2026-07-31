namespace gis.Domain.ResultPattern.Errors;

public static class DomainErrors
{
    public static class POIErrors
    {
        public static class CoordinateErrors
        {
            public static readonly Error SAME_VALUE_PROVIDED =
                new Error("POI.Coordinates.SameValue", "The provided coordinates are the same as the current ones.");

            public static readonly Error BAD_CREDENTIALS_FOR_COORDINATES =
                new Error("POI.Coordinates.BadCredentials", "Invalid coordinates credentials provided.");

            public static readonly Error LATITUDE_COORDINATE_IS_OVER_MAX_VALUE_ERROR =
                new Error("POI.Coordinates.LatitudeExceededMax", "Latitude coordinate value is over the maximum limit.");

            public static readonly Error LATITUDE_COORDINATE_IS_UNDER_MIN_VALUE_ERROR =
                new Error("POI.Coordinates.LatitudeBelowMin", "Latitude coordinate value is under the minimum limit.");

            public static readonly Error LONGITUDE_COORDINATE_IS_OVER_MAX_VALUE_ERROR =
                new Error("POI.Coordinates.LongitudeExceededMax", "Longitude coordinate value is over the maximum limit.");

            public static readonly Error LONGITUDE_COORDINATE_IS_UNDER_MIN_VALUE_ERROR =
                new Error("POI.Coordinates.LongitudeBelowMin", "Longitude coordinate value is under the minimum limit.");
            
            public static readonly Error ALTITUDE_COORDINATE_IS_OVER_MAX_VALUE_ERROR =
                new Error("POI.Coordinates.AltitudeExceededMax", "Altitude coordinate value is over the maximum limit.");

            public static readonly Error ALTITUDE_COORDINATE_IS_UNDER_MIN_VALUE_ERROR =
                new Error("POI.Coordinates.AltitudeBelowMin", "Altitude coordinate value is under the minimum limit.");

            public static readonly Error INVALID_ALTITUDE_COORDINATE_ERROR =
                new Error("POI.Coordinates.InvalidAltitude", "The provided altitude coordinate is invalid.");
        }

        public static class PointDescErrors
        {
            public static readonly Error LENGTH_REACHED_MAX_VALUE =
                new Error("POI.Description.MaxLengthExceeded", "Point description length reached the maximum allowed limit.");
        }

        public static class PointNameErrors
        {
            public static readonly Error BAD_CREDENTIALS_FOR_POINT_NAME =
                new Error("POI.Name.BadCredentials", "Invalid point name credentials provided.");

            public static readonly Error LENGTH_REACHED_MAX_VALUE =
                new Error("POI.Name.MaxLengthExceeded", "Point name length reached the maximum allowed limit.");

            public static readonly Error LENGTH_UNDER_MIN_VALUE =
                new Error("POI.Name.MinLengthRequired", "Point name length is under the minimum required limit.");
            
            public static readonly Error SAME_VALUE_PROVIDED =
                new Error("POI.Name.SameValue", "The provided point name is the same as the current one.");
        }

        public static class PointIDErrors
        {
            public static readonly Error EMPTY_ID_FORMAT =
                new Error("POI.Id.Empty", "Given ID is empty.");
        }
    }
}