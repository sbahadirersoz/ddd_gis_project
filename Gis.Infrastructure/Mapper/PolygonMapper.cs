using gis.Domain.Aggregates;
using gis.Domain.Entities.Coord;
using gis.Domain.Entities.Coord.Polygon;
using gis.Domain.Entities.IDs;
using gis.Domain.Entities.Information.Polygon;
using gis.Domain.Entities.WKT;

using Gis.Infrastructure.Entity;
using Gis.Infrastructure.HelperMethods;

using NetTopologySuite.Geometries;

namespace Gis.Infrastructure.Mapper;

public class PolygonMapper : IMapperContract<PolygonAggregate, PolygonEntity>
{
    public static PolygonAggregate ToDomain(PolygonEntity dbEntity)
    {
        var shell = RingToCoords(dbEntity.Polygon.Shell);
        var shellWKT = WellKnownText.Create(dbEntity.Wkt).Value;
        var holes = dbEntity.Polygon.Holes.Select(RingToCoords).ToList();
        var id = PolygonID.FromGuid(dbEntity.Id).Value;
        var aggName = PolygonName.CreateFromString(dbEntity.Name).Value;
        var aggDesc = PolygonDescription.CreateFromString(dbEntity.Desc).Value;
        var aggShell = PolygonShell.Create(shell, shellWKT).Value;
        var aggStatus = (PolygonStatus)dbEntity.Status;
        return PolygonAggregate.Reconstitute(id, aggShell, holes, aggName, aggDesc, aggStatus);
    }

    public static PolygonEntity ToDbEntity(PolygonAggregate domain)
    {
        var shellList = CoordinateFactory.FromCoordListToCoordinates(domain.Shell.Coordinates).ToArray();
        var shellRing = CommonGeometryHelpers.CoordListToLinearRing(shellList);
        var holeList = domain.Holes.Select(CoordinateFactory.FromCoordListToCoordinates);
        var holeRings = holeList.Select(holes => CommonGeometryHelpers.CoordListToLinearRing(holes.ToArray())).ToArray();
        var polygon = CommonGeometryHelpers.LinearRingsToPolygon(shellRing, holeRings);
        return PolygonEntity.FromAggAndPolygon(domain, polygon);
    }
    
    private static List<CoordinateValueObject> RingToCoords(LinearRing ring) =>
        ring.Coordinates.Select(c => CoordinateValueObject.ForPolygonCoordCreation(
            Latitude.Create(c.Y).Value, Longitude.Create(c.X).Value).Value).ToList();
}