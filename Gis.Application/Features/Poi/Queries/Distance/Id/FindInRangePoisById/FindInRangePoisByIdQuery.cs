using Gis.Application.Features.Line.Queries.Distance.ID.FindInRangeByIdQuery;
using gis.Domain.Entities.IDs;
using gis.Domain.ResultPattern;
using MediatR;

namespace Gis.Application.Features.Poi.Queries.Distance.Id.FindInRangePoisById;

public record FindInRangePoisByIdQuery(Guid  id,  double range):IRequest<Result<List<FindInRangePoisByIdQueryResponse>>>;