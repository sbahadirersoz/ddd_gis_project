using gis.Domain.Entities.IDs;
using gis.Domain.ResultPattern;
using MediatR;

namespace gis.ApplicationLayer.Features.Poi.Queries.FindPoiById;

public record FindPoiByIdQuery(PointID id):IRequest<Result<FindPoiByIdQueryResponse>>;