using gis.ApplicationLayer.Common;
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
    private readonly ITopologySuitePointContract _contract;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreatePoiCommandHandler> _logger;
    private readonly POIDomainService _service;

    public CreatePoiCommandHandler(POIDomainService service, ITopologySuitePointContract contract, IPointRepository pointRepository, ILogger<CreatePoiCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _service = service;
        _contract = contract;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<CreatePoiCommandResponse>> Handle(CreatePoiCommand request, CancellationToken cancellationToken = default)
    {
        var latLonFromPrimitives = PointAggregateVOMapper.CreateLatLonFromPrimitives(request.Latitude, request.Longitude);
        var pointDescFromPrimitives = PointAggregateVOMapper.CreatePointDescFromPrimitives(request.PointDesc );
        var pointNameFromPrimitives = PointAggregateVOMapper.CreatePointNameFromPrimitives(request.PoiName);
        _logger.LogInformation("All Primitives Converted To VO's");
        _logger.LogInformation("Check LatLon Creation");
        
        if (latLonFromPrimitives.IsFailure)
        {
            return Result<CreatePoiCommandResponse>.Failure(latLonFromPrimitives.Error);
        }
        _logger.LogInformation("Check Point  Desc Creation");
        
        if (pointDescFromPrimitives.IsFailure)
        {
            return Result<CreatePoiCommandResponse>.Failure(pointDescFromPrimitives.Error);
        }
        _logger.LogInformation("Check PointName Creation");
        
        if (pointNameFromPrimitives.IsFailure)
        {
         return    Result<CreatePoiCommandResponse>.Failure(pointNameFromPrimitives.Error);
        }
        
        _logger.LogInformation("All  VO Created Successfully");
        

        
        Latitude lat = latLonFromPrimitives.Value.Item1;
        Longitude lon = latLonFromPrimitives.Value.Item2;
        _logger.LogInformation("All  Lan Lon Refferances Addded Successfully");
        _logger.LogInformation("Attempting to Create Poi With  DomainService");
        var createResult = await _service.CreatePoiAggregate(lat,lon,pointDescFromPrimitives.Value,pointNameFromPrimitives.Value);
        _logger.LogInformation("Checking  Failure");
        if (createResult.IsFailure)
        {
          return  Result<CreatePoiCommandResponse>.Failure(createResult.Error);
        }
        _logger.LogInformation("Created Successfully Returning");
        var result = CreatePoiCommandResponse.CreateFromAgg(createResult.Value);
        return Result<CreatePoiCommandResponse>.Success(result);
    }
}