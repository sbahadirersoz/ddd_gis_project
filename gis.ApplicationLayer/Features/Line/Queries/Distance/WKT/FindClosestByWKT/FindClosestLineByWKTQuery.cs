using gis.Domain.ResultPattern;
using MediatR;

namespace gis.ApplicationLayer.Features.Line.Queries.Distance.WKT.FindClosestByWKT;

public record FindClosestLineByWKTQuery(string wkt):IRequest<Result<FindClosestLineByWKTResponse>>;