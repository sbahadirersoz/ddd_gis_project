using Gis.Application.Features.Poi.Queries.Distance.Id.FindInRangePoisById;
using gis.Domain.ResultPattern;
using MediatR;

namespace Gis.Application.Features.Line.Queries.Distance.ID.FindInRangeByIdQuery;

public record FindInRangeLinesByIdQuery(Guid id  , double distance):IRequest<Result<List<FindInRangeLinesByIdResponse>>>;