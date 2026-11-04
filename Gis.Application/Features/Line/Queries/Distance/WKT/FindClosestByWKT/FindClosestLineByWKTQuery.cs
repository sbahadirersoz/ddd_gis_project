using gis.Domain.ResultPattern;
using MediatR;

namespace Gis.Application.Features.Line.Queries.Distance.WKT.FindClosestByWKT;

public record FindClosestLineByWKTQuery(string wkt):IRequest<Result<FindClosestLineByWKTResponse>>;