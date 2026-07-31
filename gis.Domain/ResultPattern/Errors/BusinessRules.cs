namespace gis.Domain.ResultPattern.Errors;

public static class BusinessRules
{

    public static class POIRules
    {
        public static readonly Error DistanceBellowMin = new Error("BusinessRules.POI.Distance", "Distance between two point  should be higher than  10 meters");
    }
}