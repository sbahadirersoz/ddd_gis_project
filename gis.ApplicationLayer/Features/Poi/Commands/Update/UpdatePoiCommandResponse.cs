using gis.Domain.Aggregates;

namespace gis.ApplicationLayer.Features.Poi.Commands.Update;

public record UpdatePoiCommandResponse
{
    public string NewName { get;  }
    public string NewDesc { get; }
    public string NewWKT { get; }

    private UpdatePoiCommandResponse( string newName, string newDesc,  string newWkt)
    {
        NewName = newName;
        NewDesc = newDesc;
        NewWKT = newWkt;
    }

    public static UpdatePoiCommandResponse CreateFromAggregate(POIAggregate agg)
    {
        return new UpdatePoiCommandResponse(agg.PointName.Value, agg.PointDesc.Value, agg.CoordinateValueObject.WKT.Value);
    }
    
}
    ;