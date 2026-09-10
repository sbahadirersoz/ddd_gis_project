using System.Runtime.InteropServices.JavaScript;
using FluentAssertions;
using gis.ApplicationLayer.Common;
using gis.ApplicationLayer.Dtos;
using gis.ApplicationLayer.Features.Line.Commands.Create;
using gis.ApplicationLayer.Features.Line.Commands.Delete;
using gis.Domain.Contracts;
using gis.Domain.Entities.Coord;
using gis.Domain.Entities.IDs;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern;
using gis.Domain.Services;
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
        _contract = Substitute.For<ITopologySuiteWKTContract>();
        _uow = Substitute.For<IUnitOfWork>();
        _service = new LineDomainServiceContract(_repository, _contract);
        _handler = new DeleteLineByIdCommandHandler(_logger, _uow, _repository, _service);
    }

    [Fact]
    public async Task DeleteLineByIdSuccessCase()
    {
        var mockCreateLineCommand = new CreateLineCommand(
            coordinates: new List<CoordinateDto>
            {
                new(29.0601, 40.9902), 
                new(28.9784, 41.0082), 
                new(29.0122, 41.0428)  
            },
            lineName: "Bosphorus",
            lineDescription: "Main transit corridor connecting shoreline stations."
        );


        _contract.CreateWktStringFromCoordList(Arg.Any<List<CoordinateValueObject>>()).Returns(Result<string>.Success("POLYGON (45,25)"));
        var mockLine = await LineHelperMethods.CreateMockLine(mockCreateLineCommand);
        _repository.FindByIdAsync(Arg.Any<LineID>()).Returns(mockLine.Value);
        var mockComm = new  DeleteLineByIdCommand(LineID.New());
        var handle = await _handler.Handle(mockComm);
        if (handle.IsFailure)
            Console.WriteLine (JsonSerializer.Serialize(handle.Error));
        Console.WriteLine(JsonSerializer.Serialize(handle.Value));
        
    }
}