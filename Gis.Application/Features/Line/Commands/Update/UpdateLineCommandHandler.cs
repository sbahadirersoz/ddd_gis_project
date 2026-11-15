using Gis.Application.Common;
using Gis.Application.Mapper.LineMapper;
using gis.Domain.Contracts;
using gis.Domain.Entities.Coord;
using gis.Domain.Entities.IDs;
using gis.Domain.Entities.Information;
using gis.Domain.Entities.Information.Line;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;
using gis.Domain.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Gis.Application.Features.Line.Commands.Update;

public class UpdateLineCommandHandler:IRequestHandler<UpdateLineCommand,Result<UpdateLineCommandResponse>>
{
    private readonly ILineRepository _repository;
    private readonly ILogger<UpdateLineCommandHandler> _logger;
    private readonly ILineDomainServiceContract _service;
    private readonly IUnitOfWork _uow;
    public UpdateLineCommandHandler(ILineDomainServiceContract service, ILogger<UpdateLineCommandHandler> logger, ILineRepository repository, IUnitOfWork uow)
    {
        _service = service;
        _logger = logger;
        _repository = repository;
        _uow = uow;
    }

    public async Task<Result<UpdateLineCommandResponse>> Handle(UpdateLineCommand request, CancellationToken cancellationToken  = default)
    {
        var id = LineID.FromGuid(request.id);
        if (id.IsFailure)return Result<UpdateLineCommandResponse>.Failure(id.Error);
        var entity = await _repository.FindByIdAsync(id.Value, cancellationToken: cancellationToken ,tracking: false);
        if (entity == null)
         return   Result<UpdateLineCommandResponse>.Failure(RepositoryErrors.ENTITY_NOT_FOUND);

        LineName? name = null;
        LineDescription? desc = null;
        List<CoordinateValueObject>? coords = null;
        LineStatus? lineStatus = null;
        if (request.Coordinates != null)
        {
           coords =  request.Coordinates.Select(x => LineAggregateVOMapper.PrimitivesToCordinate(x.Latitude, x.Longitude).Value).ToList();
        }
        if (request.LineName != null)
        {
            name = LineAggregateVOMapper.PrimitiveToLineName(request.LineName).Value;
        }
        if (request.LineDescription != null)
        {
            var primitiveToLineDesc = LineAggregateVOMapper.PrimitiveToLineDesc(request.LineDescription);
            if (primitiveToLineDesc.IsFailure)  return Result<UpdateLineCommandResponse>.Failure(primitiveToLineDesc.Error);
            desc = primitiveToLineDesc.Value;
        }
        if (request.Status != null)
        {
            lineStatus  = (LineStatus)request.Status;
        }
        
        
        var updateLineAggregate = await _service.UpdateLineAggregate(entity,coords,name,desc,lineStatus);
         if (updateLineAggregate.IsFailure) return Result<UpdateLineCommandResponse>.Failure(updateLineAggregate.Error);
         _repository.Update(updateLineAggregate.Value);
         await _uow.SaveChangesAsync(cancellationToken);
            return Result<UpdateLineCommandResponse>.Success(UpdateLineCommandResponse.FromAgg(updateLineAggregate.Value));
    }
}