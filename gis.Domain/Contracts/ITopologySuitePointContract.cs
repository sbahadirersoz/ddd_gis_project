using gis.Domain.Entities.Coord;

namespace gis.Domain.Contracts;

public interface ITopologySuitePointContract
{
    string CreateWktStringFromLatLon(Latitude latitude, Longitude longitude);
}