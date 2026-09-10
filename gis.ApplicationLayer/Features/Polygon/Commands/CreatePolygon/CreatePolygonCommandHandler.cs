using gis.ApplicationLayer.Common;
using gis.ApplicationLayer.Mapper.PolygonMapper;
using gis.Domain.Contracts;
using gis.Domain.Entities.WKT;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern;
using MediatR;

namespace gis.ApplicationLayer.Features.Polygon.Commands;

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
        
        var wktStr = _wktContract.CreateWktStringFromCoordList(list);
        if (wktStr.IsFailure) return Result<CreatePolygonCommandResponse>.Failure(wktStr.Error);
        
        var wkt = WellKnownText.Create(wktStr.Value);
        if (wkt.IsFailure) return Result<CreatePolygonCommandResponse>.Failure(wkt.Error);
        
        var shell = PolygonAggregateVOMapper.CoordinateDtoListToPolygonShell(request.coords,wkt.Value);
        
        var name = PolygonAggregateVOMapper.CreatePolyNameFromString(request.Name) ;
        var desc = PolygonAggregateVOMapper.CreatePolyDescFromString(request.desc) ;
        if (shell.IsFailure) return Result<CreatePolygonCommandResponse>.Failure(shell.Error);
        if (name.IsFailure) return Result<CreatePolygonCommandResponse>.Failure(name.Error);
        if (desc.IsFailure) return Result<CreatePolygonCommandResponse>.Failure(desc.Error);
        var polygonAggregate = await _service.CreatePolygonAggregate(shell.Value,name.Value,desc.Value);
        
        if (polygonAggregate.IsFailure)
        {
            return Result<CreatePolygonCommandResponse>.Failure(polygonAggregate.Error);
        }
        await _repository.AddAsync(polygonAggregate.Value, cancellationToken);
        await UOW.SaveChangesAsync(cancellationToken);

        return Result<CreatePolygonCommandResponse>.Success(CreatePolygonCommandResponse.Create(polygonAggregate.Value));
    }
}