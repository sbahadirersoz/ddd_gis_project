using Gis.Application.Dtos;
using gis.Domain.ResultPattern;
using MediatR;

namespace Gis.Application.Features.Poi.Commands.Create;

public record CreatePoiCommand(string PoiName , string? PointDesc, CoordinateDto CoordinateDto):IRequest<Result<CreatePoiCommandResponse>>;
