using gis.Domain.Aggregates;

namespace Gis.Application.Features.Line.Commands.Create;

public record CreateLineCommandResponse(string name , string wkt , string desc,Guid id)
{
    public static CreateLineCommandResponse FromAgg(LineAggregate agg)
        => new(agg.LineName.Value, agg.WKT.Value, agg.LineDescription.Value,agg.Id.Value);
}