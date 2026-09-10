using gis.Domain.Entities.IDs;
using gis.Domain.ResultPattern;
using MediatR;

namespace gis.ApplicationLayer.Features.Line.Commands.Delete;

public record DeleteLineByIdCommand(LineID id):IRequest<Result<DeleteLineByIdCommandResponse>>;