using Gis.Application.Dtos;

using gis.Domain.Aggregates;

namespace Gis.Application.Features.Polygon.Commands.AddHoleToPolygon;

public record AddHoleToPolygonResponse(Guid id, List<List<CoordinateDto>> holes)

{
    
    public  static AddHoleToPolygonResponse  CreateFromAgg(PolygonAggregate agg)
    {
        var coordVoToDto = agg.Holes
            .Select(ring => ring.Select(x => new CoordinateDto(x.Longitude.Value, x.Latitude.Value)).ToList()).ToList();
        return new AddHoleToPolygonResponse(agg.Id.Value
            , coordVoToDto);
    }
};
