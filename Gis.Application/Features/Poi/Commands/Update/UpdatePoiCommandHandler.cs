using Gis.Application.Common;
using Gis.Application.Mapper.PoiAggregate;

using gis.Domain.Contracts;
using gis.Domain.Entities.Coord;
using gis.Domain.Entities.IDs;
using gis.Domain.Entities.Information;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;
using gis.Domain.Services;

using MediatR;

using Microsoft.Extensions.Logging;

namespace Gis.Application.Features.Poi.Commands.Update;

public class UpdatePoiCommandHandler : IRequestHandler<UpdatePoiCommand, Result<UpdatePoiCommandResponse>>
{
    private readonly POIDomainService _service;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPointRepository _pointRepository;
    private readonly ILogger<UpdatePoiCommandHandler> _logger;

    public UpdatePoiCommandHandler(ILogger<UpdatePoiCommandHandler> logger, POIDomainService service, IUnitOfWork unitOfWork, IPointRepository pointRepository)
    {
        _logger = logger;
        _service = service;
        _unitOfWork = unitOfWork;
        _pointRepository = pointRepository;
    }

    public async Task<Result<UpdatePoiCommandResponse>> Handle(UpdatePoiCommand request,
        CancellationToken cancellationToken = default)
    {
        PointName? newName = null;
        PointDescription? newDesc = null;
        Longitude? newLon = null;
        Latitude? newLat = null;
        if (request.PoiName != null)
        {
            var nameResult = PointAggregateVOMapper.CreatePointNameFromPrimitives(request.PoiName);
            if (nameResult.IsFailure)
                return Result<UpdatePoiCommandResponse>.Failure(nameResult.Error);

            newName = nameResult.Value;
        }
        
        if (request.PointDesc != null)
        {
            var descResult = PointAggregateVOMapper.CreatePointDescFromPrimitives(request.PointDesc);
            if (descResult.IsFailure)
                return Result<UpdatePoiCommandResponse>.Failure(descResult.Error);
            newDesc = descResult.Value;
        }

        if (request.CoordinateDto != null)
        {
            var coordResult = PointAggregateVOMapper.CreateLonLatFromPrimitive(request.CoordinateDto.Longitude,request.CoordinateDto.Latitude);
            if (coordResult.IsFailure)
                return Result<UpdatePoiCommandResponse>.Failure(coordResult.Error);
            newLon = coordResult.Value.Item1;
            newLat = coordResult.Value.Item2;
        }

        

        var findByEntityExpression = await _pointRepository.FindPointByIdAsync(PointID.FromGuid(request.id).Value,
            tracking: false, cancellationToken: cancellationToken);
        if (findByEntityExpression == null)
        {
            return Result<UpdatePoiCommandResponse>.Failure(RepositoryErrors.ENTITY_NOT_FOUND);
        }

        var updatedPoiAgg = await _service.UpdatePoiAggregate(findByEntityExpression, newLat, newLon, newDesc, newName, request.Status);
        
        if (updatedPoiAgg.IsFailure)
        {
            return Result<UpdatePoiCommandResponse>.Failure(updatedPoiAgg.Error);
        }

        _pointRepository.Update(updatedPoiAgg.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<UpdatePoiCommandResponse>.Success(
            UpdatePoiCommandResponse.CreateFromAggregate(updatedPoiAgg.Value));
    }
}