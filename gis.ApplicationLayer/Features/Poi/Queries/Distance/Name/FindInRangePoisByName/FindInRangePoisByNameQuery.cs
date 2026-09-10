using gis.Domain.Entities.Information;
using gis.Domain.ResultPattern;
using MediatR;

namespace gis.ApplicationLayer.Features.Poi.Queries.Distance.FindInRangePoisByName;

public record FindInRangePoisByNameQuery(string name, double distance):IRequest<Result<List<FindInRangePoisByNameQueryResponse>>>;