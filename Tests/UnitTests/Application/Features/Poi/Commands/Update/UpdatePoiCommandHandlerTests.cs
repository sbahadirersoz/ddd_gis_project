using System.Globalization;
using System.Linq.Expressions;
using System.Text.Json;
using FluentAssertions;
using gis.ApplicationLayer.Common;
using gis.ApplicationLayer.Features.Poi.Commands;
using gis.ApplicationLayer.Features.Poi.Commands.Create;
using gis.ApplicationLayer.Features.Poi.Commands.Update;
using gis.Domain.Aggregates;
using gis.Domain.Contracts;
using gis.Domain.Entities.Coord;
using gis.Domain.Entities.IDs;
using gis.Domain.Entities.Information;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;
using gis.Domain.Services;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Tests.HelperMethods.Poi;
using Xunit.Abstractions;

namespace Tests.UnitTests.Application.Features.Poi;

public class UpdatePoiCommandHandlerTests
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly ITopologySuitePointContract _contract;
    private readonly POIDomainService _service;
    private readonly IPointRepository _repository;
    private readonly UpdatePoiCommandHandler _handler;
    private readonly ILogger<UpdatePoiCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public UpdatePoiCommandHandlerTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _logger = Substitute.For<ILogger<UpdatePoiCommandHandler>>();
        _repository = Substitute.For<IPointRepository>();
        _contract = Substitute.For<ITopologySuitePointContract>();
        _unitOfWork  = Substitute.For<IUnitOfWork>();
        _service = new POIDomainService(_repository,_contract);
        _handler = new UpdatePoiCommandHandler(_logger,_service,_contract, _unitOfWork);
        
    }

    [Fact]
    public async Task UpdatePoiCommandHandler_Success_Case()
    {
        var mockPrevPoiCreation = await PoiHelperTestMethods.MockPrevPoiCreation("TestName","Description",42,42);
        //     "TestName",
        //     "Description",
        //     42,
        //     42

        var updatePoiCommand = new UpdatePoiCommand
        (
                mockPrevPoiCreation.Id,"UpdatedTestName","UpdatedTestDescription",42,15,POIStatus.ACTIVE
        );
        var latitude = Latitude.Create(updatePoiCommand.Latitude.Value);
        var lon = Longitude.Create(updatePoiCommand.Longitude.Value);
        _contract.CreateWktStringFromLatLon(latitude.Value,lon.Value).Returns(Result<string>.Success($"POINT ({latitude.Value} {lon.Value})"));
        _unitOfWork.PointRepository.FindByEntityExpression(Arg.Any<Expression<Func<POIAggregate,bool>>>()).Returns(mockPrevPoiCreation);
        var response = await _handler.Handle(updatePoiCommand);
        if (response.IsFailure)
        {
            _testOutputHelper.WriteLine(response.Error.Code);
            _testOutputHelper.WriteLine(response.Error.Desc);
        }
        response.IsSuccess.Should().BeTrue();
        
        _logger.LogInformation("Created Successfully");
        _testOutputHelper.WriteLine(response.Value.NewDesc);
        _testOutputHelper.WriteLine(response.Value.NewName);
        _testOutputHelper.WriteLine(response.Value.NewWKT);
    }
    [Fact]
    public async Task UpdatePoiCommandHandler_NullName_Failure_Case()
    {
        var mockPrevPoiCreation = await PoiHelperTestMethods.MockPrevPoiCreation("PrevName","Description",42,42);
        //     "TestName",
        //     "Description",
        //     42,
        //     42

        var updatePoiCommand = new UpdatePoiCommand
        (
                mockPrevPoiCreation.Id,"","UpdatedTestDescription",42,15,POIStatus.ACTIVE
        );
        var latitude = Latitude.Create(updatePoiCommand.Latitude.Value);
        var lon = Longitude.Create(updatePoiCommand.Longitude.Value);
        _contract.CreateWktStringFromLatLon(latitude.Value,lon.Value).Returns(Result<string>.Success($"POINT ({latitude.Value} {lon.Value})"));
        _unitOfWork.PointRepository.FindByEntityExpression(Arg.Any<Expression<Func<POIAggregate,bool>>>()).Returns(mockPrevPoiCreation);
        var response = await _handler.Handle(updatePoiCommand);
        if (response.IsFailure)
        {
            _testOutputHelper.WriteLine(response.Error.Code);
            _testOutputHelper.WriteLine(response.Error.Desc);
            return;
        }
        response.IsSuccess.Should().BeFalse();
        if (response.IsSuccess)
        {
            _logger.LogInformation("Created Successfully");
            _testOutputHelper.WriteLine(response.Value.NewDesc);
            _testOutputHelper.WriteLine(response.Value.NewName);
            _testOutputHelper.WriteLine(response.Value.NewWKT);
        }
            
        _logger.LogInformation($"Failed Reason {response.Error.Code}");
    }

    [Fact]
    public async Task? UpdatePoiCommandHandler_NothingChanged_Failure_Case()
    {
        var mockPrevPoiCreation = await PoiHelperTestMethods.MockPrevPoiCreation("PrevName","Description",42,42);
        //     "TestName",
        //     "Description",
        //     42,
        //     42

        var updatePoiCommand = new UpdatePoiCommand
        (
                mockPrevPoiCreation.Id,"PrevName","Description",42,42,POIStatus.ACTIVE
        );
        var latitude = Latitude.Create(updatePoiCommand.Latitude.Value);
        var lon = Longitude.Create(updatePoiCommand.Longitude.Value);
        _contract.CreateWktStringFromLatLon(latitude.Value,lon.Value).Returns(Result<string>.Success($"POINT ({latitude.Value} {lon.Value})"));
        _unitOfWork.PointRepository.FindByEntityExpression(Arg.Any<Expression<Func<POIAggregate,bool>>>()).Returns(mockPrevPoiCreation);
        var response = await _handler.Handle(updatePoiCommand);
        response.Error.Should().Be(DomainServiceErrors.SAME_CREDENTIALS_FOR_UPDATING);
        if (response.IsFailure)
            
        {
            _testOutputHelper.WriteLine(response.Error.Code);
            _testOutputHelper.WriteLine(response.Error.Desc);
            return;
        }
        response.IsSuccess.Should().BeFalse();
        if (response.IsSuccess)
        {
            _logger.LogInformation("Created Successfully");
            _testOutputHelper.WriteLine(response.Value.NewDesc);
            _testOutputHelper.WriteLine(response.Value.NewName);
            _testOutputHelper.WriteLine(response.Value.NewWKT);
        }
            
        _logger.LogInformation($"Failed Reason {response.Error.Code}");
    }  [Fact]
    public async Task? UpdatePoiCommandHandler_PoiNotFound_Failure_Case()
    {
        var mockPrevPoiCreation = await PoiHelperTestMethods.MockPrevPoiCreation("PrevName","Description",42,42);
        //     "TestName",
        //     "Description",
        //     42,
        //     42

        var updatePoiCommand = new UpdatePoiCommand
        (
                mockPrevPoiCreation.Id,"PrevName","Description",42,42,POIStatus.ACTIVE
        );
        var latitude = Latitude.Create(updatePoiCommand.Latitude.Value);
        var lon = Longitude.Create(updatePoiCommand.Longitude.Value);
        _contract.CreateWktStringFromLatLon(latitude.Value,lon.Value).Returns(Result<string>.Success($"POINT ({latitude.Value} {lon.Value})"));
        
        var response = await _handler.Handle(updatePoiCommand);
        response.Error.Should().Be(RepositoryErrors.ENTITY_NOT_FOUND);
        if (response.IsFailure)
            
        {
            _testOutputHelper.WriteLine(response.Error.Code);
            _testOutputHelper.WriteLine(response.Error.Desc);
            return;
        }
        response.IsSuccess.Should().BeFalse();
        if (response.IsSuccess)
        {
            _logger.LogInformation("Created Successfully");
            var json = JsonSerializer.Serialize( response.Value);
            _testOutputHelper.WriteLine(json);
        }
            
        _logger.LogInformation($"Failed Reason {response.Error.Code}");
    }
}