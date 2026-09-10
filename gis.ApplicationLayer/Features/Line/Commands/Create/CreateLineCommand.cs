using gis.ApplicationLayer.Dtos;
using gis.Domain.Entities.Information.Line;
using gis.Domain.ResultPattern;
using MediatR;

namespace gis.ApplicationLayer.Features.Line.Commands.Create;

public record CreateLineCommand(List<CoordinateDto> coordinates, string lineName, string? lineDescription):IRequest<Result<CreateLineCommandResponse>>;