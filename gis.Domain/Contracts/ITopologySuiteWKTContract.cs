using gis.Domain.Entities.Coord;
using gis.Domain.Entities.WKT;
using gis.Domain.ResultPattern;

namespace gis.Domain.Contracts;

public interface ITopologySuiteWKTContract
{
    Result<string> CreateWktStringFromLonLat( Longitude longitude,Latitude latitude);
    Result<string> CreateWktStringFromCoordList(List<CoordinateValueObject> coords);
    Result<WellKnownText> ParsePointWkt(string wkt);
}