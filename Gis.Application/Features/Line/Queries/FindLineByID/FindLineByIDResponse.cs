using gis.Domain.Aggregates;
using gis.Domain.Entities.Information;

namespace Gis.Application.Features.Line.Queries.FindLineByID;

public record FindLineByIDResponse(string  wkt  , string name,Guid  id , LineStatus status  , string desc )

{
    public static FindLineByIDResponse FromAgg(LineAggregate agg) =>
        new(agg.WKT.Value, agg.LineName.Value,agg.Id.Value,agg.LineStatus,agg.LineDescription.Value);
};