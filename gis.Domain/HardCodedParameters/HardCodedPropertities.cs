namespace gis.Domain.HardCodedParameters;

public static class HardCodedPropertities
{
    /// <summary>
    /// Daha spesifik hatalar dönebilmek için düz bir şekilde conditionlara sokmak daha önemli olduğu için es geçtik
    /// Clean code > fast code
    /// </summary>
    
    public static class BusinessRuleParameters
    {
             public const double MinDistanceBetweenPoints = 0.1;
    }

    public static class CoordinatePropertities
    {
        public const double MinLatitude = -90.0;
        public const double MaxLatitude = 90.0;
        public const double MaxLongitude = 180.0;
        public const double MinLongitude = -180.0;
        public const double MaxAltitude = 10000;
        public const double MinAltitude = -1000.0;
    }

    public static class PointDescriptionPropertities
    {
        public const int MaxLength = 255;
    }
    public static class PointNamePropertities
    {
        public const int MinLength = 3;
        public const int MaxLength = 12;
    }
}