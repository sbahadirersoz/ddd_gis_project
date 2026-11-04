using Gis.Application.Dtos;
using gis.Domain.ResultPattern;
using MediatR;

namespace Gis.Application.Features.Line.Queries.Distance.LonLat.FindClosestByLonLat;

public record FindClosestLineByLonLatQuery(CoordinateDto LonLat ):IRequest<Result<FindClosestLineByLonLatResponse>>;