using gis.Domain.Entities.Information;
using gis.Domain.ResultPattern;
using MediatR;

namespace Gis.Application.Features.Poi.Queries.Distance.FindInRangePoisByName;

public record FindInRangePoisByNameQuery(string name, double DistanceByMeter):IRequest<Result<List<FindInRangePoisByNameQueryResponse>>>;