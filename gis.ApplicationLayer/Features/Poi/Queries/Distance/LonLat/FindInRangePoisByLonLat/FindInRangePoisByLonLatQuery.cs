using gis.ApplicationLayer.Features.Poi.Queries.Distance.LonLat.FindClosestPoiByLonLat;
using gis.Domain.ResultPattern;
using MediatR;

namespace gis.ApplicationLayer.Features.Poi.Queries.Distance.LonLat.FindInRangePoisByLonLat;

public record FindInRangePoisByLonLatQuery(double lon, double lat,double distance):IRequest<Result<List<FindInRangePoisByLonLatResponse>>>;