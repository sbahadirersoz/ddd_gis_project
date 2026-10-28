using gis.Domain.ResultPattern;
using MediatR;

namespace Gis.Application.Features.Line.Queries.Distance.Name.FindInRangeByName;

public record FindInRangeByNameQuery(string name,double DistanceInMeter):IRequest<Result<List<FindInRangeByNameResponse>>>;