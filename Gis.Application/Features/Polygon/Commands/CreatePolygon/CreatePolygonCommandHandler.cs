using Gis.Application.Common;
using Gis.Application.Mapper.PolygonMapper;

using gis.Domain.Contracts;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern;

using MediatR;

namespace Gis.Application.Features.Polygon.Commands;

public class CreatePolygonCommandHandler:IRequestHandler<CreatePolygonCommand,Result<CreatePolygonCommandResponse>>
{

    private readonly IPolygonRepository _repository;
    private readonly IUnitOfWork UOW;
    private readonly IPolygonDomainServiceContract _service;
    private readonly ITopologySuiteWKTContract _wktContract;

    public CreatePolygonCommandHandler(IPolygonRepository repository, IUnitOfWork uow, IPolygonDomainServiceContract service, ITopologySuiteWKTContract wktContract)
    {
        _repository = repository;
        UOW = uow;
        _service = service;
        _wktContract = wktContract;
    }

    public async Task<Result<CreatePolygonCommandResponse>> Handle(CreatePolygonCommand request, CancellationToken cancellationToken = default)
    {

        var list = PolygonAggregateVOMapper.DtoToValueObjects(request.coords);
        if (list.IsFailure) return Result<CreatePolygonCommandResponse>.Failure(list.Error);

        
        var name = PolygonAggregateVOMapper.CreatePolyNameFromString(request.Name) ;
        if (name.IsFailure) return Result<CreatePolygonCommandResponse>.Failure(name.Error);
        
        var desc = PolygonAggregateVOMapper.CreatePolyDescFromString(request.desc) ;
        if (desc.IsFailure) return Result<CreatePolygonCommandResponse>.Failure(desc.Error);
        
        var polygonAggregate = await _service.CreatePolygonAggregate(list.Value!,name.Value!,desc.Value);
        
        if (polygonAggregate.IsFailure)
        {
            return Result<CreatePolygonCommandResponse>.Failure(polygonAggregate.Error);
        }
        await _repository.AddAsync(polygonAggregate.Value!, cancellationToken);
        await UOW.SaveChangesAsync(cancellationToken);

        return Result<CreatePolygonCommandResponse>.Success(CreatePolygonCommandResponse.Create(polygonAggregate.Value!));
    }
}