using gis.Domain.Aggregates;

namespace Gis.Application.Features.Line.Queries.FindLineByID;

public record FindLineByIDResponse(string  wkt  , string name )

{
    public static FindLineByIDResponse FromAgg(LineAggregate agg) =>
        new(agg.WKT.Value, agg.LineName.Value);
};