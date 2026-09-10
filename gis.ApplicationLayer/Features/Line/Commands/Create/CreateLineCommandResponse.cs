using gis.Domain.Aggregates;

namespace gis.ApplicationLayer.Features.Line.Commands.Create;

public record CreateLineCommandResponse(string name , string wkt , string desc)
{
    public static CreateLineCommandResponse FromAgg(LineAggregate agg)
        => new(agg.LineName.Value, agg.WKT.Value, agg.LineDescription.Value);
}