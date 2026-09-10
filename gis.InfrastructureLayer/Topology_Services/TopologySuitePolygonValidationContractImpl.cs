using gis.Domain.Contracts;
using gis.Domain.Entities.Coord;
using gis.Domain.Entities.Coord.Polygon;
using gis.Domain.ResultPattern;
using gis.InfrastructureLayer.HelperMethods;
using gis.Domain.ResultPattern.Errors;
using NetTopologySuite.Geometries;

namespace gis.InfrastructureLayer.Topology_Services;

public class TopologySuitePolygonValidationContractImpl:ITopologySuitePolygonValidationContract
{
    public Result IsHoleInsideTheShell(List<CoordinateValueObject> hole, PolygonShell shell)
    {
        try
        {
            if (hole == null || hole.Count < 3 || shell == null || shell.Coordinates == null || shell.Coordinates.Count < 3)
                return Result.Failure(BusinessRuleErrors.PolygonRules.POLYGON_NOT_CLOSED);

            var shellCoords = CoordinateFactory.FromCoordListToCoordinates(shell.Coordinates);
            var holeCoords = CoordinateFactory.FromCoordListToCoordinates(hole);

            // ensure rings are closed (first == last)
            if (!shellCoords.First().Equals(shellCoords.Last()))
                shellCoords.Add(new Coordinate(shellCoords.First().X, shellCoords.First().Y));
            if (!holeCoords.First().Equals(holeCoords.Last()))
                holeCoords.Add(new Coordinate(holeCoords.First().X, holeCoords.First().Y));

            var gf = NetTopologySuite.NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
            var shellRing = gf.CreateLinearRing(shellCoords.ToArray());
            var holeRing = gf.CreateLinearRing(holeCoords.ToArray());

            if (!shellRing.IsValid || !holeRing.IsValid)
                return Result.Failure(BusinessRuleErrors.PolygonRules.POLYGON_NOT_CLOSED);

            var polygon = gf.CreatePolygon(shellRing, new LinearRing[] { holeRing });
            var holePolygon = gf.CreatePolygon(holeRing);

            // strict containment: hole must be fully inside shell (not touching boundary)
            if (polygon.Contains(holePolygon))
                return Result.Success();

            return Result.Failure(BusinessRuleErrors.PolygonRules.POLYGON_NOT_CLOSED);
        }
        catch (Exception)
        {
            return Result.Failure(BusinessRuleErrors.UNEXPECTED_ERROR);
        }
    }


}