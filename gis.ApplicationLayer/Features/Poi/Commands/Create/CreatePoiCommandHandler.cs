using gis.Domain.Contracts;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern;
using gis.Domain.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace gis.ApplicationLayer.Features.Poi.Commands;

public class CreatePoiCommandHandler:IRequestHandler<CreatePoiCommand,Result<CreatePoiCommandResponse>>
{
    private readonly IPointRepository _pointRepository;
    private readonly ITopologySuitePointContract _contract;
    private readonly ILogger<CreatePoiCommandHandler> _logger;
    private readonly POIDomainService _service;
    public async Task<Result<CreatePoiCommandResponse>> Handle(CreatePoiCommand request, CancellationToken cancellationToken)
    {
        return Result<CreatePoiCommandResponse>.Success(new CreatePoiCommandResponse());
    }
}