using Gis.Application.Features.Poi.Queries.Distance.LonLat.FindClosestPoiByLonLat;
using gis.Domain.ResultPattern;
using MediatR;

namespace Gis.Application.Features.Poi.Queries.Distance.LonLat.FindInRangePoisByLonLat;

public record FindInRangePoisByLonLatQuery(double lon, double lat,double distance):IRequest<Result<List<FindInRangePoisByLonLatResponse>>>;