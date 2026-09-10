using gis.ApplicationLayer.Common;
using gis.ApplicationLayer.Mapper.LineMapper;
using gis.ApplicationLayer.Mapper.PoiAggregate;
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

namespace gis.ApplicationLayer.Features.Line.Commands.Update;

public class UpdateLineCommandHandler:IRequestHandler<UpdateLineCommand,Result<UpdateLineCommandResponse>>
{
    private readonly ILineRepository _repository;
    private readonly ITopologySuiteWKTContract _contract;
    private readonly ILogger<UpdateLineCommandHandler> _logger;
    private readonly ILineDomainServiceContract _service;
    private readonly IUnitOfWork _uow;
    public UpdateLineCommandHandler(ILineDomainServiceContract service, ILogger<UpdateLineCommandHandler> logger, ITopologySuiteWKTContract contract, ILineRepository repository, IUnitOfWork uow)
    {
        _service = service;
        _logger = logger;
        _contract = contract;
        _repository = repository;
        _uow = uow;
    }

    public async Task<Result<UpdateLineCommandResponse>> Handle(UpdateLineCommand request, CancellationToken cancellationToken)
    {
        var id = LineID.FromGuid(request.id);
        if (id.IsFailure)return Result<UpdateLineCommandResponse>.Failure(id.Error);
        var entity = await _repository.FindByIdAsync(id.Value, cancellationToken: cancellationToken ,tracking: false);
        if (entity == null)
         return   Result<UpdateLineCommandResponse>.Failure(RepositoryErrors.ENTITY_NOT_FOUND);
        var coordinates = entity.Coordinates;
            
        if (request.Coordinates != null)
        {
           coordinates =  request.Coordinates.Select(x => LineAggregateVOMapper.PrimitivesToCordinate(x.Latitude, x.Longitude).Value).ToList();
        }
        
        
        var lineName = LineAggregateVOMapper.PrimitiveToLineName(request.LineName);
        if (lineName.IsFailure) return Result<UpdateLineCommandResponse>.Failure(lineName.Error);
        
        
        var lineDesc = LineAggregateVOMapper.PrimitiveToLineDesc(request.LineDescription);
        if (lineDesc.IsFailure) return Result<UpdateLineCommandResponse>.Failure(lineDesc.Error);


        var lineStatus = entity.LineStatus;
        if (request.Status != null)
        {
          lineStatus =(LineStatus)request.Status;
        }
            

        var updateLineAggregate = await _service.UpdateLineAggregate(entity,coordinates,lineName.Value,lineDesc.Value,lineStatus);
         if (updateLineAggregate.IsFailure) return Result<UpdateLineCommandResponse>.Failure(updateLineAggregate.Error);
         _repository.Update(updateLineAggregate.Value);
         await _uow.SaveChangesAsync(cancellationToken);
            return Result<UpdateLineCommandResponse>.Success(UpdateLineCommandResponse.FromAgg(updateLineAggregate.Value));
    }
}