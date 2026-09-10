using gis.Domain.ResultPattern;
using MediatR;

namespace gis.ApplicationLayer.Features.Line.Queries.Distance.WKT.FindMostClosesCountByWKT;

public record FindMostClosesLineCountByWKTQuery(string wkt ,int count ):IRequest<Result<List<FindMostClosesLineCountByWKTResponse>>>;