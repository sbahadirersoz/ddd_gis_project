using gis.ApplicationLayer.Mapper.PoiAggregate;
using gis.Domain.Contracts;
using gis.Domain.Entities.Coord;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern;
using gis.Domain.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace gis.ApplicationLayer.Features.Poi.Commands.Create;

public class CreatePoiCommandHandler:IRequestHandler<CreatePoiCommand,Result<CreatePoiCommandResponse>>
{
    private readonly IPointRepository _pointRepository;
    private readonly ITopologySuitePointContract _contract;
    private readonly ILogger<CreatePoiCommandHandler> _logger;
    private readonly POIDomainService _service;
    public async Task<Result<CreatePoiCommandResponse>> Handle(CreatePoiCommand request, CancellationToken cancellationToken)
    {
        var latLonFromPrimitives = PointAggregateVOMapper.CreateLatLonFromPrimitives(request.Latitude, request.Longitude);
        var pointDescFromPrimitives = PointAggregateVOMapper.CreatePointDescFromPrimitives(request.PointDesc );
        var pointNameFromPrimitives = PointAggregateVOMapper.CreatePointNameFromPrimitives(request.PoiName);
        if (latLonFromPrimitives.IsFailure)
        {
            Result<CreatePoiCommandResponse>.Failure(latLonFromPrimitives.Error);
        }
        if (pointDescFromPrimitives.IsFailure)
        {
            Result<CreatePoiCommandResponse>.Failure(pointDescFromPrimitives.Error);
        }
        if (pointNameFromPrimitives.IsFailure)
        {
            Result<CreatePoiCommandResponse>.Failure(pointNameFromPrimitives.Error);
        }

        Latitude lat = latLonFromPrimitives.Value.Item1;
        Longitude lon = latLonFromPrimitives.Value.Item2;
        var createResult = await _service.CreatePoiAggregate(lat,lon,pointDescFromPrimitives.Value,pointNameFromPrimitives.Value);
        if (createResult.IsFailure)
        {
            Result<CreatePoiCommandResponse>.Failure(createResult.Error);
        }
        return Result<CreatePoiCommandResponse>.Success(CreatePoiCommandResponse.CreateFromAgg(createResult.Value));
    }
}