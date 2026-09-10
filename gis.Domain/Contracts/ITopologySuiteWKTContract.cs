using gis.Domain.Entities.Coord;
using gis.Domain.ResultPattern;

namespace gis.Domain.Contracts;

public interface ITopologySuiteWKTContract
{
    Result<string> CreateWktStringFromLonLat(Latitude latitude, Longitude longitude);
    Result<string> CreateWktStringFromCoordList(List<CoordinateValueObject> coords);
}