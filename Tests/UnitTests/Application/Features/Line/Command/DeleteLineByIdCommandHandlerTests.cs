using System.Runtime.InteropServices.JavaScript;
using FluentAssertions;
using Gis.Application.Common;
using Gis.Application.Dtos;
using Gis.Application.Features.Line.Commands.Create;
using Gis.Application.Features.Line.Commands.Delete;
using gis.Domain.Contracts;
using gis.Domain.Entities.Coord;
using gis.Domain.Entities.IDs;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern;
using gis.Domain.Services;

using Gis.Infrastructure.Topology_Services;

using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NSubstitute;
using Tests.HelperMethods.Line;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Tests.UnitTests.Application.Features.Line.Command;

public class DeleteLineByIdCommandHandlerTests
{
    private readonly ILineRepository _repository;
    private readonly ILogger<DeleteLineByIdCommandHandler> _logger;
    private readonly ITopologySuiteWKTContract _contract;
    private readonly IUnitOfWork _uow;
    private readonly LineDomainServiceContract _service;
    private readonly DeleteLineByIdCommandHandler _handler;
    

    public DeleteLineByIdCommandHandlerTests()
    {
        _repository = Substitute.For<ILineRepository>();
        _logger = Substitute.For<ILogger<DeleteLineByIdCommandHandler>>();
        _contract =new TopologySuiteWktContractImpl();
        _uow = Substitute.For<IUnitOfWork>();
        _service = new LineDomainServiceContract(_repository, _contract);
        _handler = new DeleteLineByIdCommandHandler(_logger, _uow, _repository, _service);
    }

    [Fact]
    public async Task DeleteLineByIdSuccessCase()
    {


        var mockLine = await LineHelperMethods.CreateMockLine(new CoordinateDto(42,35),new CoordinateDto(42,75),"linename","lineDesc");
        _repository.FindByIdAsync(Arg.Any<LineID>()).Returns(mockLine.Value);
        var mockComm = new  DeleteLineByIdCommand(LineID.New());
        var handle = await _handler.Handle(mockComm);
        if (handle.IsFailure)
            Console.WriteLine (JsonSerializer.Serialize(handle.Error));
        Console.WriteLine(JsonSerializer.Serialize(handle.Value));
        
    }
}