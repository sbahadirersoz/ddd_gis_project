using gis.Domain.ResultPattern;
using MediatR;

namespace gis.ApplicationLayer.Features.Poi.Commands;

public record CreatePoiCommand(string PoiName, string PointDesc, double Latitude, double Longitude):IRequest<Result<CreatePoiCommandResponse>>;