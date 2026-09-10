using gis.Domain.Common;
using gis.Domain.Entities.Coord;
using gis.Domain.Entities.Information;
using gis.Domain.Entities.Information.Line;
using gis.Domain.Entities.WKT;

namespace gis.Domain.Entities.Events.LineEvents;

public record LineStringUpdatedEvent(LineName prevName,List<CoordinateValueObject> prevCoordinates, LineDescription? prevDesc, LineStatus prevStatus, WellKnownText prevWkt, LineName name, List<CoordinateValueObject> coordinates, LineDescription? lineDescription, LineStatus lineStatus, WellKnownText wkt):IDomainEvent
{
    public static LineStringUpdatedEvent Create(LineName prevName,List<CoordinateValueObject> prevCoordinates, LineDescription? prevDesc,
        LineStatus prevStatus, WellKnownText prevWkt, LineName name, List<CoordinateValueObject> coordinates,
        LineDescription? lineDescription, LineStatus lineStatus, WellKnownText wkt)
        => new( prevName,prevCoordinates, prevDesc, prevStatus, prevWkt, name,coordinates, lineDescription, lineStatus, wkt);
}