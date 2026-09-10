using gis.Domain.ResultPattern;
using MediatR;

namespace gis.ApplicationLayer.Features.Poi.Queries.Distance.LonLat.FindClosestPoiByLonLat;

public record FindClosestPoiByLonLatQuery(double  lon, double lat):IRequest<Result<FindClosestPoiByLonLatResponse>>;