using Gis.Application.Dtos;
using gis.Domain.ResultPattern;
using MediatR;

namespace Gis.Application.Features.Line.Queries.Distance.LonLat.FindInRangeByLonLat;

public record FindInRangeLineByLonLatQuery(CoordinateDto LonLat , double DistanceInMeter):IRequest<Result<List<FindInRangeLineByLonLatResponse>>>;