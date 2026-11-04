using gis.Domain.ResultPattern;
using MediatR;

namespace Gis.Application.Features.Line.Queries.Distance.WKT.FindMostClosesCountByWKT;

public record FindMostClosesLineCountByWKTQuery(string wkt ,int count ):IRequest<Result<List<FindMostClosesLineCountByWKTResponse>>>;