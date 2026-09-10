using gis.Domain.Entities.WKT;
using gis.Domain.ResultPattern;
using MediatR;

namespace gis.ApplicationLayer.Features.Poi.Queries.Distance.Wkt.FindInRangePoisByWkt;

public record FindInRangePoisByWktQuery(string wkt,double distance):IRequest<Result<List<FindInRangePoisByWktResponse>>>;