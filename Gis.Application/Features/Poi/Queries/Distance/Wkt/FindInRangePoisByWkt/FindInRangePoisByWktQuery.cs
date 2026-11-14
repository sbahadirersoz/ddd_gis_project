using gis.Domain.Entities.WKT;
using gis.Domain.ResultPattern;
using MediatR;

namespace Gis.Application.Features.Poi.Queries.Distance.Wkt.FindInRangePoisByWkt;

public record FindInRangePoisByWktQuery(string wkt,double DistanceByMeter):IRequest<Result<List<FindInRangePoisByWktResponse>>>;