namespace gis.Domain.ResultPattern.Errors;

public static class DomainErrors
{
    
    public static readonly Error EMPTY_ID_FORMAT =
        new Error("EntityID.Empty", "Given ID is empty.");
    public static readonly Error BAD_CREDENTIALS_FOR_NAME =
        new Error("EntityName.BadCredentials", "Invalid  name credentials provided.");

    public static readonly Error LONGITUDE_COORDINATE_IS_NOT_IN_RANGE_ERROR =
        new Error("Entity.Coordinates.LongitudeNotInRange", "Longitude coordinate value is not in the range.");

    
       public static readonly Error LATITUDE_COORDINATE_IS_NOT_IN_RANGE_ERROR =
        new Error("Entity.Coordinates.LatitudeNotInRange", "Latitude coordinate value is not in the range.");
       public static readonly Error BAD_CREDENTIALS_FOR_STATUS =
           new Error("Entity.Status.BadCredentials", "Invalid  status credentials provided.");

       public static class WKT
       {
           
           public static readonly Error INVALID_WKT_FORMAT =
               new Error("WKT.FormatInvalid", "Given WKT Format is invalid.");
       }

    
    
    public static class POIErrors
    {

        public static readonly Error INVALID_PARAMETER_FOR_CLONING =
            new Error("POI.Cloning.InvalidParameter", "The provided coordinates are the same as the current ones.");
      public static readonly Error ALREADY_SOFTDELETED =
            new Error("POI.SoftDeleted.AlreadySoftDeleted", "This Entity is Already SoftDeleted.");

        public static class Coordinate
        {
        public static readonly Error INSUFFICIENT_COORDINATES_FOR_LINE = new Error("POI.Coordinates.InsufficientCoordinates", "Insufficient coordinates.");
            
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

        public static class PointDesc
        {
            public static readonly Error LENGTH_REACHED_MAX_VALUE =
                new Error("POI.Description.MaxLengthExceeded", "Point description length reached the maximum allowed limit.");
        }

        public static class PointName
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

        public static class PointID
        {
            public static readonly Error EMPTY_ID_FORMAT =
                new Error("POI.Id.Empty", "Given ID is empty.");
        }

        public static class WKT
        {
            public static readonly Error INVALID_WKT_FORMAT =
                new Error("POI.WKT.FormatInvalid", "Given WKT Format is invalid.");
        }   
        }

    public static class LineErrors
    {
    
        
        public static readonly Error INVALID_UPDATE_PARAMETERS =
            new Error("LINE.Update.BadCredentials", "Invalid line  credentials provided.");

            
        public static class Name
        {
            public static readonly Error BAD_CREDENTIALS_FOR_LINE_NAME =
                new Error("LINE.Name.BadCredentials", "Invalid line name credentials provided.");

            public static readonly Error LENGTH_REACHED_MAX_VALUE =
                new Error("LINE.Name.MaxLengthExceeded", "Line name length reached the maximum allowed limit.");

            public static readonly Error LENGTH_UNDER_MIN_VALUE =
                new Error("LINE.Name.MinLengthRequired", "Line name length is under the minimum required limit.");
            
            public static readonly Error SAME_VALUE_PROVIDED =
                new Error("LINE.Name.SameValue", "The provided line name is the same as the current one.");
        }
        public static class Coordinates
        {
            public static readonly Error BAD_CREDENTIALS_FOR_CORDINATES =
                new Error("LINE.Coordinates.BadCredentials", "Invalid line name credentials provided.");

            public static readonly Error COORDINATES_ARE_EMPTY =
                new Error("LINE.Coordinates.Emtpy", "Line coordinates are empty");

            public static readonly Error LENGTH_UNDER_MIN_VALUE =
                new Error("LINE.Name.MinLengthRequired", "Line name length is under the minimum required limit.");
            
            public static readonly Error SAME_VALUE_PROVIDED =
                new Error("LINE.Name.SameValue", "The provided line name is the same as the current one.");
        }
        public static class WKT
        {
            public static readonly Error INVALID_WKT_FORMAT =
                new Error("POI.WKT.FormatInvalid", "Given WKT Format is invalid.");
        }
    
    }
    }
