using gis.Domain.ResultPattern;
using MediatR;

namespace Gis.Application.Features.Line.Queries.Distance.WKT.FindInRangeByWKT;

public record FindInRangeLineByWKTQuery(string wkt , double DistanceByMeter):IRequest<Result<List<FindInRangeLineByWKTResponse>>>;