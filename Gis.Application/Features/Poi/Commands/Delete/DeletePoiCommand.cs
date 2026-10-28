using gis.Domain.Entities.IDs;
using gis.Domain.ResultPattern;
using MediatR;

namespace Gis.Application.Features.Poi.Commands.Delete;

public record DeletePoiCommand(PointID id):IRequest<Result<DeletePoiCommandResponse>>;