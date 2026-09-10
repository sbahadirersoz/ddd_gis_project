using gis.ApplicationLayer.Common;
using gis.Domain.Contracts;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;
using gis.Domain.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace gis.ApplicationLayer.Features.Line.Commands.Delete;

public class DeleteLineByIdCommandHandler:IRequestHandler<DeleteLineByIdCommand,Result<DeleteLineByIdCommandResponse>>
{
    private readonly  ILogger<DeleteLineByIdCommandHandler> _logger;
    private readonly  IUnitOfWork _uow;
    private readonly  ILineRepository _repository;
    private readonly  ILineDomainServiceContract _serviceContract;

    public DeleteLineByIdCommandHandler(ILogger<DeleteLineByIdCommandHandler> logger, IUnitOfWork uow, ILineRepository repository, ILineDomainServiceContract serviceContract)
    {
        _logger = logger;
        _uow = uow;
        _repository = repository;
        _serviceContract = serviceContract;
    }

    public async Task<Result<DeleteLineByIdCommandResponse>> Handle(DeleteLineByIdCommand request, CancellationToken cancellationToken = default)
    {
        var entity =await _repository.FindByIdAsync(request.id, cancellationToken: cancellationToken);
        if (entity == null)
        {
            _logger.LogInformation($"Null Found {request.id}");
            return Result<DeleteLineByIdCommandResponse>.Failure(RepositoryErrors.ENTITY_NOT_FOUND);
        }
        _serviceContract.SoftDelete(entity);

        await _uow.SaveChangesAsync(cancellationToken);
        _logger.LogInformation($"Deleted Line {request.id}");
        return Result<DeleteLineByIdCommandResponse>.Success(DeleteLineByIdCommandResponse.FromAgg(entity));
    }
}