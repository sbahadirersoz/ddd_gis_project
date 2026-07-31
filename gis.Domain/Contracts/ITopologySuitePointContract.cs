using gis.Domain.Entities.Coord;

namespace gis.Domain.Contracts;

public interface ITopologySuitePointContract
{
    double CheckDistanceBetweenPoints(Coordinates point1, Coordinates point2);
}