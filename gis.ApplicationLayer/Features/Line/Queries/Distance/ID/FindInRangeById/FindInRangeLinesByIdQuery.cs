using gis.ApplicationLayer.Features.Poi.Queries.Distance.Id.FindInRangePoisById;
using gis.Domain.ResultPattern;
using MediatR;

namespace gis.ApplicationLayer.Features.Line.Queries.Distance.ID.FindInRangeByIdQuery;

public record FindInRangeLinesByIdQuery(Guid id  , double distance):IRequest<Result<List<FindInRangeLinesByIdResponse>>>;