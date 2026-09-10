using gis.ApplicationLayer.Dtos;
using gis.Domain.Entities.IDs;
using gis.Domain.Entities.Information;
using gis.Domain.ResultPattern;
using MediatR;

namespace gis.ApplicationLayer.Features.Line.Commands.Update;

public record UpdateLineCommand(Guid id ,string? LineName  = null, string?  LineDescription =  null,List<CoordinateDto?>Coordinates = null,LineStatus? Status = null ):IRequest<Result<UpdateLineCommandResponse>>;