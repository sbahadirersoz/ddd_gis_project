using gis.Domain.Aggregates;

namespace Gis.Application.Features.Line.Commands.Delete;

public record DeleteLineByIdCommandResponse(string wkt , string name ,string desc)
{
    public static DeleteLineByIdCommandResponse FromAgg(LineAggregate entity)
    => new (entity.WKT.Value,entity.LineName.Value,entity.LineDescription.Value);
}

