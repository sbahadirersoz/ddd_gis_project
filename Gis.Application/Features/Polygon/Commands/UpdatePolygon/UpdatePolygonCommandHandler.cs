using Gis.Application.Common;

using gis.Domain.Contracts;
using gis.Domain.Entities.IDs;
using gis.Domain.Entities.Information.Polygon;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;

using MediatR;

namespace Gis.Application.Features.Polygon.Commands.UpdatePolygon;

public class UpdatePolygonCommandHandler : IRequestHandler<UpdatePolygonCommand, Result<UpdatePolygonCommandResponse>>
{
    private readonly IPolygonDomainServiceContract _service;
    private readonly IUnitOfWork _uow;
    private readonly IPolygonRepository _repository;

    public UpdatePolygonCommandHandler(IPolygonDomainServiceContract service, IUnitOfWork uow, IPolygonRepository repository)
    {
        _service = service;
        _uow = uow;
        _repository = repository;
    }

    public async Task<Result<UpdatePolygonCommandResponse>> Handle(UpdatePolygonCommand request, CancellationToken cancellationToken)
    {
        var polyId = PolygonID.FromGuid(request.id);
        if (polyId.IsFailure) return Result<UpdatePolygonCommandResponse>.Failure(polyId.Error);
        var entity = await _repository.FindByIdAsync(polyId.Value!,tracking:false, cancellationToken: cancellationToken);
        if (entity == null) return Result<UpdatePolygonCommandResponse>.Failure(RepositoryErrors.ENTITY_NOT_FOUND);
        PolygonName? name = null;
        PolygonDescription? description = null;
        PolygonStatus? status = null;

        if (request.desc != null)
        {
            var polyDesc = PolygonDescription.CreateFromString(request.desc);
            if (polyDesc.IsFailure) return Result<UpdatePolygonCommandResponse>.Failure(polyDesc.Error);
            description = polyDesc.Value!;
        }
        if (request.name != null)
        {
            var polyName =  PolygonName.CreateFromString(request.name);
            if (polyName.IsFailure) return Result<UpdatePolygonCommandResponse>.Failure(polyName.Error);
            name = polyName.Value!;
        }
        if (request.status != null)
        {
            status = (PolygonStatus)request.status;
        }

        var updatePoly =  await _service.UpdatePolygonAggregate(entity, name, description, status);
        if (updatePoly.IsFailure) return Result<UpdatePolygonCommandResponse>.Failure(updatePoly.Error);
        _repository.Update(updatePoly.Value);
        await _uow.SaveChangesAsync(cancellationToken);
        return Result<UpdatePolygonCommandResponse>.Success(UpdatePolygonCommandResponse.FromAgg(updatePoly.Value));

    }
}
