using System.Text.Json;
using System.Text.Json.Serialization;
using FluentAssertions;
using gis.ApplicationLayer.Common;
using gis.ApplicationLayer.Dtos;
using gis.ApplicationLayer.Features.Poi.Commands.Create;
using gis.Domain.Contracts;
using gis.Domain.Entities.Coord;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern;
using gis.Domain.Services;
using gis.InfrastructureLayer.Topology_Services;
using Microsoft.Extensions.Logging;
using NSubstitute;
using gis.Domain.Entities.Information;
using Xunit.Abstractions;

namespace Tests.UnitTests.Application.Features.Poi;

public class CreatePoiCommandHandlerTests
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly ITopologySuiteWKTContract _contract;
    private readonly POIDomainService _service;
    private readonly IPointRepository _repository;
    private readonly CreatePoiCommandHandler _handler;
    private readonly ILogger<CreatePoiCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePoiCommandHandlerTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _logger = Substitute.For<ILogger<CreatePoiCommandHandler>>();
        _repository = Substitute.For<IPointRepository>();
        _contract = Substitute.For<ITopologySuiteWKTContract>();
        _unitOfWork  = Substitute.For<IUnitOfWork>();
        _service = new POIDomainService(_repository,_contract);
        _handler = new CreatePoiCommandHandler(_service, _contract,_logger, _unitOfWork,_repository);
    }

    [Fact]
    public async Task CreatePoiCommandHandler_SuccessCase()
    {
        
        var mockCommand = new CreatePoiCommand
        (
            "TestName",
            "Description",
           new CoordinateDto(42,35)
        );
        
        var lat = Latitude.Create(mockCommand.CoordinateDto.Latitude).Value;
        var lon = Longitude.Create(mockCommand.CoordinateDto.Longitude).Value;
        _repository.IsLonLatCoordinatesExistsAsync(lat, lon).Returns(false);
        _repository.IsPointNameExistsAsync(Arg.Any<PointName>()).Returns(false);
        _contract.CreateWktStringFromLonLat(lat, lon).Returns(Result<string>.Success($"POINT ({lat.Value} {lon.Value})"));
        var result = await _handler.Handle(mockCommand, CancellationToken.None);
        if (result.IsFailure)
        {
            _testOutputHelper.WriteLine(result.Error.Code);
            return;
        }
        string  json = JsonSerializer.Serialize(result.Value);
        _testOutputHelper.WriteLine(json);
    }
         
      
    
    }
