using gis.ApplicationLayer.Common;
using gis.ApplicationLayer.Features.Line.Commands.Delete;
using gis.ApplicationLayer.Features.Line.Commands.Update;
using gis.Domain.Contracts;
using gis.Domain.Repositories;
using gis.Domain.Services;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Tests.UnitTests.Application.Features.Line.Command;

public class UpdateLineCommandHandlerTests
{
    
    private readonly ILineRepository _repository;
    private readonly ILogger<UpdateLineCommandHandler> _logger;
    private readonly ITopologySuiteWKTContract _contract;
    private readonly IUnitOfWork _uow;
    private readonly LineDomainServiceContract _service;
    private readonly UpdateLineCommandHandler _handler;

    public UpdateLineCommandHandlerTests()
    {
        _repository = Substitute.For<ILineRepository>();
        _logger = Substitute.For<ILogger<UpdateLineCommandHandler>>();
        _contract = Substitute.For<ITopologySuiteWKTContract>();
        _uow = Substitute.For<IUnitOfWork>();
        _service = new LineDomainServiceContract(_repository,_contract);
        _handler= new UpdateLineCommandHandler(_service,_logger,_contract,_repository,_uow);
    }

    [Fact]
    public async Task UpdateLineCommand()
    {
        
    }
}