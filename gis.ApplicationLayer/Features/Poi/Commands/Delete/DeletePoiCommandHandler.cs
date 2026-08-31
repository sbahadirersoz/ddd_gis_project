using gis.ApplicationLayer.Common;
using gis.ApplicationLayer.Features.Poi.Commands.Create;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;
using gis.Domain.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace gis.ApplicationLayer.Features.Poi.Commands.Delete;

public class DeletePoiCommandHandler:IRequestHandler<DeletePoiCommand,Result<DeletePoiCommandResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPointRepository _pointRepository;
    private readonly ILogger<CreatePoiCommandHandler> _logger;
    private readonly POIDomainService _service;
    public DeletePoiCommandHandler(IUnitOfWork unitOfWork, IPointRepository pointRepository, ILogger<CreatePoiCommandHandler> logger, POIDomainService service)
    {
        _unitOfWork = unitOfWork;
        _pointRepository = pointRepository;
        _logger = logger;
        _service = service;
    }

    public async Task<Result<DeletePoiCommandResponse>> Handle(DeletePoiCommand request,
        CancellationToken cancellationToken = default)
    {
        var findEntityByIdAsync =await _pointRepository.FindPointByIdAsync(request.id, true,cancellationToken: cancellationToken);
        if (findEntityByIdAsync == null)
            return Result<DeletePoiCommandResponse>.Failure(RepositoryErrors.ENTITY_NOT_FOUND);
        
        var softDeletePoi = _service.SoftDeletePoi(findEntityByIdAsync);
        if (softDeletePoi.IsFailure)
            return Result<DeletePoiCommandResponse>.Failure(softDeletePoi.Error);
        
        await _pointRepository.UpdateAsync(findEntityByIdAsync, cancellationToken);
        var affectedRows = await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Affected rows on SoftDelete: {Rows}", affectedRows);
        return softDeletePoi.IsFailure
            ? Result<DeletePoiCommandResponse>.Failure(softDeletePoi.Error)
            : Result<DeletePoiCommandResponse>.Success(DeletePoiCommandResponse.CreateFromAgg(findEntityByIdAsync));
    }
}