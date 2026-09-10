namespace gis.API.EndPoints;

public static class Endpoints
{
    public const string Base = "api/v1";

    public static class Pois
    {
        public const string Root = Base + "/pois";
        public const string List = "";
        public const string GetById =  "{id:guid}";
        public const string Create = "create";
        public const string Update = "";
        public const string PartialUpdate = "/{id:guid}";
        public const string Delete =  "{id:guid}";          
        
        public const string MostClosesByCountID = "closesByCount";
        public const string MostClosesByCountName = "closesByNameCount";   
        public const string MostClosesByCountWkt = "closesByWktCount";   
        public const string MostClosesByCountLatLon = "closesByLatLon";   
        
        
        public const string FindClosestById = "closestById";
        public const string FindClosestByName = "closestByName";
        public const string FindClosestByWkt = "closestByWkt";
        public const string FindClosestByLatLon = "closestByLatLon";                
        
        public const string Search =  "search";               
        public const string inRange =  "inRange";             
        public const string inRangeByName =  "inRange/byName";
        public const string inRangeByWkt =  "inRange/byWkt";
        public const string inRangeByLatLon =  "inRange/byLatLon";                
    }

    public static class Lines
    {
        public const string Root = Base + "/lines";
        public const string inRange =  "inRange";
        public const string MostClosesByCountID = "closesByCount";                
        public const string FindClosestById = "closestById";                
        
        public const string List = "";                              
        public const string GetById =  "{id:guid}";                 
        public const string Create = "create";                              
        public const string Update = "";
        public const string PartialUpdate = "/{id:guid}";           
        public const string Delete =  "{id:guid}";          

        
        
    }
}