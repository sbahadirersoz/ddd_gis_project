using gis.ApplicationLayer.Dtos;
using gis.Domain.ResultPattern;
using MediatR;

namespace gis.ApplicationLayer.Features.Line.Queries.Distance.LonLat.FindInRangeByLonLat;

public record FindInRangeLineByLonLatQuery(CoordinateDto LonLat , double DistanceInMeter):IRequest<Result<List<FindInRangeLineByLonLatResponse>>>;