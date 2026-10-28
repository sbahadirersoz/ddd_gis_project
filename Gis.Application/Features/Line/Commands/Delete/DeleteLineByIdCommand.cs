using gis.Domain.Entities.IDs;
using gis.Domain.ResultPattern;
using MediatR;

namespace Gis.Application.Features.Line.Commands.Delete;

public record DeleteLineByIdCommand(LineID id):IRequest<Result<DeleteLineByIdCommandResponse>>;