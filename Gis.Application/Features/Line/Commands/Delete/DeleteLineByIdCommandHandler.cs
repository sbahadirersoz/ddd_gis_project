using Gis.Application.Common;
using gis.Domain.Contracts;
using gis.Domain.Entities.IDs;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;
using gis.Domain.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Gis.Application.Features.Line.Commands.Delete;

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
        var id = LineID.FromGuid(request.id);
        if (id.IsFailure) return Result<DeleteLineByIdCommandResponse>.Failure(RepositoryErrors.ENTITY_NOT_FOUND);
        var entity =await _repository.FindByIdAsync(id.Value,false ,cancellationToken: cancellationToken);
        if (entity == null)
        {
            _logger.LogInformation($"Null Found {request.id}");
            return Result<DeleteLineByIdCommandResponse>.Failure(RepositoryErrors.ENTITY_NOT_FOUND);
        }

        Result softDelete = _serviceContract.SoftDelete(entity);
        if (softDelete.IsFailure) return Result<DeleteLineByIdCommandResponse>.Failure(softDelete.Error);
        _repository.Update(entity);
        await _uow.SaveChangesAsync(cancellationToken);
        _logger.LogInformation($"Deleted Line {request.id}");
        return Result<DeleteLineByIdCommandResponse>.Success(DeleteLineByIdCommandResponse.FromAgg(entity));
    }
}