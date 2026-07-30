namespace gis.Domain.ResultPattern.Errors;

public static class DomainErrors
{
    public static class PointOfInterestErrors
    {
        public static class CoordinateErrors
        {
            public static readonly Error SAME_VALUE_PROVIDED =
                new Error("BAD_CREDENTIALS_FOR_COORDINATES", "SAME_VALUE_PROVIDED");
public static readonly Error BAD_CREDENTIALS_FOR_COORDINATES =
                new Error("BAD_CREDENTIALS_FOR_COORDINATES", "BAD_CREDENTIALS_FOR_COORDINATES");

            public static readonly Error LATITUDE_CORDINATE_IS_OVER_MAX_VALUE_ERROR =
                new Error("INVALID_CORDINATE_ERROR", "LATITUDE_CORDINATE_IS_OVER_MAX_VALUE_ERROR");

            public static readonly Error LATITUDE_CORDINATE_IS_UNDER_MIN_VALUE_ERROR =
                new Error("INVALID_CORDINATE_ERROR", "LATITUDE_CORDINATE_IS_UNDER_MIN_VALUE_ERROR");

            public static readonly Error LONGITUDE_CORDINATE_IS_OVER_MAX_VALUE_ERROR =
                new Error("INVALID_CORDINATE_ERROR", "LONGITUDE_CORDINATE_IS_OVER_MAX_VALUE_ERROR");

            public static readonly Error LONGITUDE_CORDINATE_IS_UNDER_MIN_VALUE_ERROR =
                new Error("INVALID_CORDINATE_ERROR", "LONGITUDE_CORDINATE_IS_UNDER_MIN_VALUE_ERROR");
            public static readonly Error ALTIDUTE_CORDINATE_IS_OVER_MAX_VALUE_ERROR =
                new Error("INVALID_CORDINATE_ERROR", "ALTIDUTE_CORDINATE_IS_OVER_MAX_VALUE_ERROR");

            public static readonly Error ALTIDUTE_CORDINATE_IS_UNDER_MIN_VALUE_ERROR =
                new Error("INVALID_CORDINATE_ERROR", "ALTIDUTE_CORDINATE_IS_UNDER_MIN_VALUE_ERROR");


            public static readonly Error INVALID_ALTITUDE_CORDINATE_ERROR =
                new Error("INVALID_CORDINATE_ERROR", "    INVALID_LONGITUDE_CORDINATE_ERROR");
        }

        public static class PointDescErrors
        {
            public static readonly Error LENGTH_REACHED_MAX_VALUE =
                new Error("POINT_DESC_LENGTH_REACHED_MAX_VALUE", "POINT_DESC_LENGTH_REACHED_MAX_VALUE");
        }

        public static class PointNameErrors
        {
            public static readonly Error BAD_CREDENTIALS_FOR_POINT_NAME =
                new Error("BAD_CREDENTIALS_FOR_POINT_NAME", "BAD_CREDENTIALS_FOR_POINT_NAME");

            public static readonly Error LENGTH_REACHED_MAX_VALUE =
                new Error("BAD_CREDENTIALS_FOR_POINT_NAME", "BAD_CREDENTIALS_FOR_POINT_NAME");

            public static readonly Error LENGTH_UNDER_MIN_VALUE =
                new Error("BAD_CREDENTIALS_FOR_POINT_NAME", "LENGTH_UNDER_MIN_VALUE");
            
            public static readonly Error SAME_VALUE_PROVIDED =
                new Error("BAD_CREDENTIALS_FOR_POINT_NAME", "SAME_VALUE_PROVIDED");
            
            
        }
        public static class PointIDErrors
        {
            public static readonly Error EMPTY_ID_FORMAT =
                new Error("EMPTY_ID_FORMAT", "Given ID is empty");
                
        }
    }
}