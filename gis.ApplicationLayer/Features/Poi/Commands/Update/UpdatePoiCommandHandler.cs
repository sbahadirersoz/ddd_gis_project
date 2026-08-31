using gis.ApplicationLayer.Common;
using gis.ApplicationLayer.Mapper.PoiAggregate;
using gis.Domain.Contracts;
using gis.Domain.Entities.IDs;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;
using gis.Domain.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace gis.ApplicationLayer.Features.Poi.Commands.Update;

public class UpdatePoiCommandHandler:IRequestHandler<UpdatePoiCommand,Result<UpdatePoiCommandResponse>>
{
    private readonly POIDomainService _service;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPointRepository _pointRepository;
    private readonly ITopologySuiteWKTContract _contract;
    private readonly ILogger<UpdatePoiCommandHandler> _logger;
    public UpdatePoiCommandHandler(ILogger<UpdatePoiCommandHandler> logger, POIDomainService service, ITopologySuiteWKTContract contract, IUnitOfWork unitOfWork, IPointRepository pointRepository)
    {
        _logger = logger;
        _service = service;
        _contract = contract;
        _unitOfWork = unitOfWork;
        _pointRepository = pointRepository;
    }

    public async Task<Result<UpdatePoiCommandResponse>> Handle(UpdatePoiCommand request, CancellationToken cancellationToken = default)
    {
        var findByEntityExpression = await _pointRepository.FindPointByIdAsync(PointID.FromGuid(request.id).Value ,tracking:false, cancellationToken: cancellationToken);
        if (findByEntityExpression == null)
        {
            return Result<UpdatePoiCommandResponse>.Failure(RepositoryErrors.ENTITY_NOT_FOUND);
        }
        var latLonFromPrimitives = PointAggregateVOMapper.CreateLonLatFromPrimitive(request.Latitude.Value, request.Longitude.Value);
        var pointDescFromPrimitives = PointAggregateVOMapper.CreatePointDescFromPrimitives(request.PointDesc);
        var pointNameFromPrimitives = PointAggregateVOMapper.CreatePointNameFromPrimitives(request.PoiName);
        if (latLonFromPrimitives.IsFailure)
        {
            return Result<UpdatePoiCommandResponse>.Failure(latLonFromPrimitives.Error);
        }
        if (pointNameFromPrimitives.IsFailure)
        {
            return Result<UpdatePoiCommandResponse>.Failure(pointNameFromPrimitives.Error);
        }
        if (pointDescFromPrimitives.IsFailure)
        {
            return Result<UpdatePoiCommandResponse>.Failure(pointDescFromPrimitives.Error);
        }
        var longitude = latLonFromPrimitives.Value.Item1;
        var latitude = latLonFromPrimitives.Value.Item2;
        var updatedPoiAgg = await _service.UpdatePoiAggregate(findByEntityExpression,latitude,longitude,pointDescFromPrimitives.Value,pointNameFromPrimitives.Value,request.Status);
        if (updatedPoiAgg.IsFailure)
        {
            return Result<UpdatePoiCommandResponse>.Failure(updatedPoiAgg.Error);
        }
        _pointRepository.Update(updatedPoiAgg.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<UpdatePoiCommandResponse>.Success(UpdatePoiCommandResponse.CreateFromAggregate(updatedPoiAgg.Value));
    }
}