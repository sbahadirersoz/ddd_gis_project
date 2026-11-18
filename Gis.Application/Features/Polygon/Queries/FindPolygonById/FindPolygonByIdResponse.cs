using Gis.Application.Dtos;

using gis.Domain.Aggregates;
using gis.Domain.Entities.Information.Polygon;

namespace Gis.Application.Features.Polygon.Queries.FindPolygonById;

public record FindPolygonByIdResponse(
    string wkt,
    string name,
    Guid id,
    PolygonStatus status,
    string desc,
    List<List<CoordinateDto>> holes)
{
    public  static FindPolygonByIdResponse  CreateFromAgg(PolygonAggregate agg)
    {
        var CoordVOToDto = agg.Holes
            .Select(ring => ring.Select(x => new CoordinateDto(x.Longitude.Value, x.Latitude.Value)).ToList()).ToList();
        return new FindPolygonByIdResponse(agg.Shell.wkt.Value, agg.Name.Value, agg.Id.Value, agg.Status,
            agg.Description.Value!
            , CoordVOToDto);
    }
};
