using FluentAssertions;
using gis.ApplicationLayer.Common;
using gis.ApplicationLayer.Features.Poi.Commands.Create;
using gis.Domain.Contracts;
using gis.Domain.Entities.Coord;
using gis.Domain.Repositories;
using gis.Domain.Services;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit.Abstractions;

namespace Tests.UnitTests.Application.Features.Poi;

public class CreatePoiCommandHandlerTests
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly ITopologySuitePointContract _contract;
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
        _contract = Substitute.For<ITopologySuitePointContract>();
        _unitOfWork  = Substitute.For<IUnitOfWork>();
        _service = new POIDomainService(_repository,_contract);
        _handler = new CreatePoiCommandHandler(_service, _contract,_logger, _unitOfWork);
    }

    [Fact]
    public async Task CreatePoiCommandHandler_SuccessCase()
    {
        
        var mockCommand = new CreatePoiCommand
        (
            "TestName",
            "Description",
            42,
            42
        );
        
        var lat = Latitude.Create(mockCommand.Latitude).Value;
        var lon = Longitude.Create(mockCommand.Longitude).Value;
        _contract.CreateWktStringFromLatLon(lat,lon).Returns($"POINT ({lat.Value} {lon.Value})");
        var result = await _handler.Handle(mockCommand, CancellationToken.None);
        result.IsSuccess.Should().BeTrue();
        _testOutputHelper.WriteLine("FOR TESTING ALIAS");
    }
         
      
    
    }
