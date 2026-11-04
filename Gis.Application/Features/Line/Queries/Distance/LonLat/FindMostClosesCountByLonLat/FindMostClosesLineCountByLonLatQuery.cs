using Gis.Application.Dtos;
using gis.Domain.ResultPattern;
using MediatR;

namespace Gis.Application.Features.Line.Queries.Distance.LonLat.FindMostClosesCountByLonLat;

public record FindMostClosesLineCountByLonLatQuery(CoordinateDto lonLat,int count):IRequest<Result<List<FindMostClosesLineCountByLonLatResponse>>>;