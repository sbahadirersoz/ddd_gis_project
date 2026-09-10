using gis.Domain.ResultPattern;
using MediatR;

namespace gis.ApplicationLayer.Features.Poi.Queries.Distance.Wkt.FindMostClosesCountPoiByWkt;

public record FindMostClosesCountPoiByWktQuery(string wkt , int count):IRequest<Result<List<FindMostClosesCountPoiByWktQueryResponse>>>;