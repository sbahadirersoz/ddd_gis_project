using gis.Domain.ResultPattern;
using MediatR;

namespace gis.ApplicationLayer.Features.Poi.Queries.GetAllPois;

public record GetAllPoisQuery():IRequest<Result<IReadOnlyList<GetAllPoisQueryResponse>>>;