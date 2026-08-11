using gis.Domain.Entities.Information;
using gis.Domain.ResultPattern;
using MediatR;

namespace gis.ApplicationLayer.Features.Poi.Queries.FindPoiByName;

public record FindPoiByNameQuery:IRequest<Result<FindPoiByNameQueryResponse>>
{
    public PointName PointName { get; }
}