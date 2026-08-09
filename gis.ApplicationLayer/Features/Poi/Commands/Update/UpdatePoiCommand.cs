using gis.ApplicationLayer.Features.Poi.Commands.Update;
using gis.Domain.Entities.IDs;
using gis.Domain.Entities.Information;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;
using MediatR;

namespace gis.ApplicationLayer.Features.Poi.Commands;

public record UpdatePoiCommand
    (PointID id,
        string? PoiName, string? PointDesc, double? Latitude, double? Longitude,POIStatus? Status)
    :IRequest<Result<UpdatePoiCommandResponse>>;