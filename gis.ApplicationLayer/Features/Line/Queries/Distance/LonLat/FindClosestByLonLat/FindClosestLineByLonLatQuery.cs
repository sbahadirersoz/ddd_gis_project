using gis.ApplicationLayer.Dtos;
using gis.Domain.ResultPattern;
using MediatR;

namespace gis.ApplicationLayer.Features.Line.Queries.Distance.LonLat.FindClosestByLonLat;

public record FindClosestLineByLonLatQuery(CoordinateDto LonLat ):IRequest<Result<FindClosestLineByLonLatResponse>>;