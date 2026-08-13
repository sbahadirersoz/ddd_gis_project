using gis.Domain.Entities.Coord;
using gis.Domain.ResultPattern;

namespace gis.Domain.Contracts;

public interface ITopologySuitePointContract
{
    Result<string> CreateWktStringFromLatLon(Latitude latitude, Longitude longitude);
}