using gis.Domain.ResultPattern;
using MediatR;

namespace gis.ApplicationLayer.Features.Poi.Queries.Distance.LonLat.FindMostClosesCountPoiByLonLat;

public record FindMostClosesCountPoiByLonLatQuery(double lon, double lat,int count):IRequest<Result<List<FindMostClosesCountPoiByLonLatResponse>>>;