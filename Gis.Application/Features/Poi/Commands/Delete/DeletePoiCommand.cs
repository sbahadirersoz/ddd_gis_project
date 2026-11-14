using gis.Domain.Entities.IDs;
using gis.Domain.ResultPattern;
using MediatR;

namespace Gis.Application.Features.Poi.Commands.Delete;

public record DeletePoiCommand(Guid id):IRequest<Result<DeletePoiCommandResponse>>;