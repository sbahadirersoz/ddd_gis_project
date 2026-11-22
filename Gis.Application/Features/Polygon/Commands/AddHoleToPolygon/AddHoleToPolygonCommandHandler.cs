using Gis.Application.Common;
using Gis.Application.Mapper.PolygonMapper;

using gis.Domain.Contracts;
using gis.Domain.Entities.IDs;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;

using MediatR;

namespace Gis.Application.Features.Polygon.Commands.AddHoleToPolygon;

public class AddHoleToPolygonCommandHandler:IRequestHandler<AddHoleToPolygonCommand,Result<AddHoleToPolygonResponse>>
{

    private readonly IPolygonDomainServiceContract _service;
    private readonly IUnitOfWork _uow;
    private readonly IPolygonRepository _repository;

    public AddHoleToPolygonCommandHandler(IPolygonDomainServiceContract service, IUnitOfWork uow, IPolygonRepository repository)
    {
        _service = service;
        _uow = uow;
        _repository = repository;
    }

    public async Task<Result<AddHoleToPolygonResponse>> Handle(AddHoleToPolygonCommand request, CancellationToken cancellationToken)
    {
        var polyId = PolygonID.FromGuid(request.id);
        if (polyId.IsFailure) return Result<AddHoleToPolygonResponse>.Failure(polyId.Error);

        var agg = await _repository.FindByIdAsync(polyId.Value!,false,cancellationToken: cancellationToken);
        if (agg == null) return Result<AddHoleToPolygonResponse>.Failure(RepositoryErrors.ENTITY_NOT_FOUND);
        
        var dtoToValueObjects = PolygonAggregateVOMapper.DtoToValueObjects(request.hole);
        if (dtoToValueObjects.IsFailure) return Result<AddHoleToPolygonResponse>.Failure(dtoToValueObjects.Error);

        var result = await _service.AddHoleToPolygon(agg,dtoToValueObjects.Value!);
        if (result.IsFailure) return Result<AddHoleToPolygonResponse>.Failure(result.Error);
        
        _repository.Update(result.Value!);
         await _uow.SaveChangesAsync(cancellationToken);
         
         return Result<AddHoleToPolygonResponse>.Success(AddHoleToPolygonResponse.CreateFromAgg(result.Value));
    }
}