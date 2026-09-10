using System.Text.Json;
using FluentAssertions;
using gis.ApplicationLayer.Common;
using gis.ApplicationLayer.Dtos;
using gis.ApplicationLayer.Features.Line.Commands.Create;
using gis.Domain.Contracts;
using gis.Domain.Entities.Coord;
using gis.Domain.Entities.Information.Line;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern;
using gis.Domain.Services;
using Microsoft.Extensions.Logging;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using NSubstitute;
using Xunit.Abstractions;

namespace Tests.UnitTests.Application.Features.Line.Command;

public class CreateLineCommandHandlerTests
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly ILineDomainServiceContract _service;
    private readonly ITopologySuiteWKTContract _contract;
    private readonly ILineRepository _repository;
    private readonly CreateLineCommandHandler _handler;
    private readonly ILogger<CreateLineCommandHandler> _logger;
    private readonly IUnitOfWork _uow;

    public CreateLineCommandHandlerTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _repository = Substitute.For<ILineRepository>();
        _contract = Substitute.For<ITopologySuiteWKTContract>();
        _uow = Substitute.For<IUnitOfWork>();
        
        _logger = Substitute.For<ILogger<CreateLineCommandHandler>>();
        _service = new LineDomainServiceContract(_repository, _contract);
        _handler = new CreateLineCommandHandler(_repository,  _service,_logger,_uow);
    }
    private static readonly  GeometryFactory _geometryFactory =NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
    private static readonly WKTReader WktReader = new(_geometryFactory);
    [Fact]
    public async Task CreateLineCommandShouldCreateLine()
    {
        var mockCreateLineCommand = new CreateLineCommand(
            coordinates: new List<CoordinateDto>
            {
                new CoordinateDto(35,42),
                new CoordinateDto(44,42),
                new CoordinateDto(46,42),
                new CoordinateDto(45,43),
                
            },
            lineName: "Bosphorus",
            lineDescription: "Main transit corridor connecting shoreline stations."
        );
        
        _contract.CreateWktStringFromCoordList(Arg.Any<List<CoordinateValueObject>>()).Returns(Result<string>.Success("POLYGON (35,42)"));
            _repository.IsLineNameExistsAsync(Arg.Any<LineName>()).Returns(false);
        var handle = await _handler.Handle(mockCreateLineCommand);
        if (handle.IsFailure) _testOutputHelper.WriteLine(JsonSerializer.Serialize(handle.Error.Code));
            _testOutputHelper.WriteLine(JsonSerializer.Serialize(handle.Value));

            handle.IsSuccess.Should().Be(true);
            
    }
}