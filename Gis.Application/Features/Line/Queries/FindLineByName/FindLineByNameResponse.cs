using gis.Domain.Aggregates;

namespace Gis.Application.Features.Line.Queries.FindLineByName;

public record FindLineByNameResponse(string  wkt  , string name )

{
    public static FindLineByNameResponse FromAgg(LineAggregate agg) =>
        new(agg.WKT.Value, agg.LineName.Value );
};