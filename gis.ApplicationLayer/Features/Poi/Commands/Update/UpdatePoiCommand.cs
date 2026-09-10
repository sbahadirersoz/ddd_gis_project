using gis.ApplicationLayer.Dtos;
using gis.ApplicationLayer.Features.Poi.Commands.Update;
using gis.Domain.Entities.IDs;
using gis.Domain.Entities.Information;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;
using MediatR;

namespace gis.ApplicationLayer.Features.Poi.Commands;

public record UpdatePoiCommand
    (Guid id,
        string? PoiName = null, string? PointDesc = null, CoordinateDto? CoordinateDto = null,POIStatus? Status = null)
    :IRequest<Result<UpdatePoiCommandResponse>>;