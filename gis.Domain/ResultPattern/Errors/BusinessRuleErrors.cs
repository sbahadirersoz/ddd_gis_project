namespace gis.Domain.ResultPattern.Errors;

public static class BusinessRuleErrors
{
    
    
        public static readonly Error ENTITY_SOFT_DELETED = new Error("BusinessRules.EntityDeleted", "Entity Deleted cant be operate this method ");
    
        public static readonly Error UNEXPECTED_ERROR = new Error("BusinessRules.UnExpectedErrorOccured", "Unexpected error occured");
        public static readonly Error ENTITY_ALREADY_SOFT_DELETED = new Error("BusinessRules.Entity.Already.SoftDeleted", "Entity Already deleted.");
        public static readonly Error ENTITY_STATUS_ALREADY_GIVEN_PARAMETER = new Error("BusinessRules.Entity.Already.GivenStatus", "EntityStatus Already given param.");
        public static readonly Error THIS_FIELD_IS_UNIQUE = new Error("BusinessRules.Entity.Already.GivenStatus", "EntityStatus Already given param.");

    public static class POIRules
    {
        public static readonly Error DISTANCE_BELLOW_MIN = new Error("BusinessRules.POI.Distance", "Distance between two point  should be higher than  10 meters");
    }
    public static class LineRules
    {
        public static readonly Error INSUFFICENT_POINTS = new Error("BusinessRules.LINE.InsufficentPoints", "Insufficient points provided: expected 2, received 1");
    }

    public static class DistanceQueryRulesErrors
    {
        public static readonly Error INVALID_DISTANCE_RANGE = new Error("BusinessRules.DistanceQuery.InvalidDistanceRange", "The provided value is outside the allowed range.");
        public static readonly Error INVALID_QUERY_COUNT = new Error("BusinessRules.DistanceQuery.InvalidQueryCount", "The provided value is outside the allowed range.");
        
    }

    public static class PolygonRules
    {
        public static readonly Error POLYGON_NOT_CLOSED = new Error("BusinessRules.Polygon.BadCredentials", "The provided value doesnt comply with PolygonRules .");
        public static readonly Error INSUFFICENT_POINTS = new Error("BusinessRules.Polygon.BadCredentials", "The provided value doesnt comply with PolygonRules .");

    }
    
    
}