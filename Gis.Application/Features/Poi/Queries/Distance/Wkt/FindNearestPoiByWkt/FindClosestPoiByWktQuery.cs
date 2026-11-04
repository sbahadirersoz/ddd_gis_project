using gis.Domain.Entities.WKT;
using gis.Domain.ResultPattern;
using MediatR;

namespace Gis.Application.Features.Poi.Queries.Distance.Wkt.FindNearestPoiByWkt;

public record FindClosestPoiByWktQuery(string  wkt):IRequest<Result<FindClosestPoiByWktResponse>>;