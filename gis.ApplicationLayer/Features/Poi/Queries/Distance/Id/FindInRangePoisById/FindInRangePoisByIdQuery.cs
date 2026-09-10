using gis.ApplicationLayer.Features.Line.Queries.Distance.ID.FindInRangeByIdQuery;
using gis.Domain.Entities.IDs;
using gis.Domain.ResultPattern;
using MediatR;

namespace gis.ApplicationLayer.Features.Poi.Queries.Distance.Id.FindInRangePoisById;

public record FindInRangePoisByIdQuery(Guid  id,  double range):IRequest<Result<List<FindInRangePoisByIdQueryResponse>>>;