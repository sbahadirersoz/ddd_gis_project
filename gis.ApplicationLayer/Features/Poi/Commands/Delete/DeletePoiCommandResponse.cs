using gis.Domain.Aggregates;
using gis.Domain.Entities.WKT;
using gis.Domain.ResultPattern.ResponseMessages;

namespace gis.ApplicationLayer.Features.Poi.Commands.Delete;

public record DeletePoiCommandResponse
{
    public string Message { get; }
    public DateTime OccuredDate { get; init; } = DateTime.UtcNow;
    public string WKT { get; }
    public string Name { get; }
    public string Desc { get; }
    public string Status{ get; }

    
    
    private DeletePoiCommandResponse( string wkt, string name, string desc, string status)
    {
        Message = DomainResponseMessages.Deleted;
        WKT = wkt;
        Name = name;
        Desc = desc;
        Status = status;
    }

    public static DeletePoiCommandResponse CreateFromAgg(POIAggregate agg) =>
        new DeletePoiCommandResponse(agg.CoordinateValueObject.WKT.Value, agg.PointName.Value, agg.PointDesc.Value,
            agg.Status.ToString());
}