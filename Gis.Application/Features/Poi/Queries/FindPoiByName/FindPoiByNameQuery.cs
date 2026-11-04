using gis.Domain.Entities.Information;
using gis.Domain.ResultPattern;
using MediatR;

namespace Gis.Application.Features.Poi.Queries.FindPoiByName;

public record FindPoiByNameQuery:IRequest<Result<FindPoiByNameQueryResponse>>
{
    public PointName PointName { get; }

    public FindPoiByNameQuery(PointName pointName)
    {
        PointName = pointName;
    }
}