using gis.Domain.ResultPattern;
using MediatR;

namespace Gis.Application.Features.Poi.Queries.GetAllPois;

public record GetAllPoisQuery():IRequest<Result<IReadOnlyList<GetAllPoisQueryResponse>>>;