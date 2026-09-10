using gis.ApplicationLayer.Common;
using gis.ApplicationLayer.Mapper.LineMapper;
using gis.Domain.Contracts;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern;
using MediatR;
using Microsoft.Extensions.Logging;

namespace gis.ApplicationLayer.Features.Line.Commands.Create;

public class CreateLineCommandHandler: IRequestHandler<CreateLineCommand, Result<CreateLineCommandResponse>>
{
    private readonly ILineRepository _repository;
    private readonly ILineDomainServiceContract _service;
    private readonly ILogger<CreateLineCommandHandler> _logger ;
    private readonly IUnitOfWork _uow ;


    public CreateLineCommandHandler(ILineRepository repository, ILineDomainServiceContract service, ILogger<CreateLineCommandHandler> logger, IUnitOfWork uow)
    {
        _repository = repository;
        _service = service;
        _logger = logger;
        _uow = uow;
    }

    public async Task<Result<CreateLineCommandResponse>> Handle(CreateLineCommand request, CancellationToken cancellationToken =default)
    {
        
        var primitiveToLineName = LineAggregateVOMapper.PrimitiveToLineName(request.lineName);
        if (primitiveToLineName.IsFailure)
            return Result<CreateLineCommandResponse>.Failure(primitiveToLineName.Error);
        
        

        var primitiveToLineDesc = LineAggregateVOMapper.PrimitiveToLineDesc(request.lineDescription);
        if (primitiveToLineDesc.IsFailure)
            return Result<CreateLineCommandResponse>.Failure(primitiveToLineDesc.Error);
        
        
        

        var list = request.coordinates.Select( x=> LineAggregateVOMapper.PrimitivesToCordinate(x.Latitude,x.Longitude).Value).ToList();
        _logger.LogDebug("TEST BECOME CREATE LINE");

        var lineAggregate = await _service.CreateLineAggregate(list,primitiveToLineName.Value,primitiveToLineDesc.Value);

        if (lineAggregate.IsFailure) return Result<CreateLineCommandResponse>.Failure(lineAggregate.Error);
        await _repository.AddAsync(lineAggregate.Value, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken: cancellationToken);
            return Result<CreateLineCommandResponse>.Success(CreateLineCommandResponse.FromAgg(lineAggregate.Value));
    }
}