using gis.Domain.ResultPattern;
using MediatR;

namespace gis.ApplicationLayer.Features.Line.Queries.Distance.Name.FindInRangeByName;

public record FindInRangeByNameQuery(string name,double DistanceInMeter):IRequest<Result<List<FindInRangeByNameResponse>>>;