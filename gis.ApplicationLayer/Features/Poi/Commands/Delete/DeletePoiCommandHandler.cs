using gis.ApplicationLayer.Common;
using gis.ApplicationLayer.Features.Poi.Commands.Create;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;
using gis.Domain.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace gis.ApplicationLayer.Features.Poi.Commands.Delete;

public class DeletePoiCommandHandler:IRequestHandler<DeletePoiCommand,Result<DeletePoiCommandResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreatePoiCommandHandler> _logger;
    private readonly POIDomainService _service;
    public DeletePoiCommandHandler(IUnitOfWork unitOfWork, ILogger<CreatePoiCommandHandler> logger, POIDomainService service)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _service = service;
    }
    public async Task<Result<DeletePoiCommandResponse>> Handle(DeletePoiCommand request, CancellationToken cancellationToken)
    {
        var findEntityByIdAsync = await _unitOfWork.PointRepository.FindEntityByIdAsync(request.id);
        if (findEntityByIdAsync == null)
            return Result<DeletePoiCommandResponse>.Failure(RepositoryErrors.ENTITY_NOT_FOUND);
        var softDeletePoi = _service.SoftDeletePoi(findEntityByIdAsync);
        return softDeletePoi.IsFailure ? Result<DeletePoiCommandResponse>.Failure(softDeletePoi.Error) : Result<DeletePoiCommandResponse>.Success(DeletePoiCommandResponse.CreateFromAgg(findEntityByIdAsync));
    }
}