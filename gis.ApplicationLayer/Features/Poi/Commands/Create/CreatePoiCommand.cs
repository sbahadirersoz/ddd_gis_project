using gis.ApplicationLayer.Dtos;
using gis.Domain.ResultPattern;
using MediatR;

namespace gis.ApplicationLayer.Features.Poi.Commands.Create;

public record CreatePoiCommand(string PoiName , string? PointDesc, CoordinateDto CoordinateDto):IRequest<Result<CreatePoiCommandResponse>>;
