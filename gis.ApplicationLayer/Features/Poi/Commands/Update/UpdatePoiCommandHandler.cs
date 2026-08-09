using gis.ApplicationLayer.Common;
using gis.Domain.Aggregates;
using gis.Domain.Contracts;
using gis.Domain.Entities.IDs;
using gis.Domain.Repositories;
using gis.Domain.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace gis.ApplicationLayer.Features.Poi.Commands.Update;

public class UpdatePoiCommandHandler:IRequestHandler<UpdatePoiCommand,UpdatePoiCommandResponse>
{
    private readonly POIDomainService _service;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITopologySuitePointContract _contract;
    private readonly ILogger<UpdatePoiCommandHandler> _logger;
    public UpdatePoiCommandHandler(ILogger<UpdatePoiCommandHandler> logger, POIDomainService service, IPointRepository repo, ITopologySuitePointContract contract, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _service = service;
        _contract = contract;
        _unitOfWork = unitOfWork;
    }

    public Task<UpdatePoiCommandResponse> Handle(UpdatePoiCommand request, CancellationToken cancellationToken)
    {
        
    }
}