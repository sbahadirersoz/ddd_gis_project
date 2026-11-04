using gis.Domain.Entities.IDs;
using gis.Domain.ResultPattern;
using MediatR;

namespace Gis.Application.Features.Poi.Queries.FindPoiById;

public record FindPoiByIdQuery(PointID id):IRequest<Result<FindPoiByIdQueryResponse>>;