
using gis.Domain.Aggregates;
using gis.Domain.Entities.Coord;

namespace gis.ApplicationLayer.Features.Line.Commands.Update;

public record UpdateLineCommandResponse(string wkt,string name,string desc , List<CoordinateValueObject>coords)    
{
    public static UpdateLineCommandResponse FromAgg(LineAggregate agg) =>new(agg.WKT.Value, agg.LineName.Value, agg.LineDescription.Value,
            agg.Coordinates);
    
}
