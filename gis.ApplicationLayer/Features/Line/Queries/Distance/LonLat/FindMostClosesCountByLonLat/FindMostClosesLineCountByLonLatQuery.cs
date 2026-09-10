using gis.ApplicationLayer.Dtos;
using gis.Domain.ResultPattern;
using MediatR;

namespace gis.ApplicationLayer.Features.Line.Queries.Distance.LonLat.FindMostClosesCountByLonLat;

public record FindMostClosesLineCountByLonLatQuery(CoordinateDto lonLat,int count):IRequest<Result<List<FindMostClosesLineCountByLonLatResponse>>>;