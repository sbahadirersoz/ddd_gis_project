using gis.Domain.Entities.IDs;
using gis.Domain.ResultPattern;
using MediatR;

namespace gis.ApplicationLayer.Features.Poi.Commands.Delete;

public record DeletePoiCommand(PointID id):IRequest<Result<DeletePoiCommandResponse>>;