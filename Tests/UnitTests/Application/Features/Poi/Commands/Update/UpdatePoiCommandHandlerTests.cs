using System.Globalization;
using System.Linq.Expressions;
using System.Text.Json;
using FluentAssertions;
using gis.ApplicationLayer.Common;
using gis.ApplicationLayer.Dtos;
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
using NetTopologySuite.Geometries;
using NSubstitute;
using Tests.HelperMethods.Poi;
using Xunit.Abstractions;

namespace Tests.UnitTests.Application.Features.Poi;

public class UpdatePoiCommandHandlerTests
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly ITopologySuiteWKTContract _contract;
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
        _contract = Substitute.For<ITopologySuiteWKTContract>();
        _unitOfWork  = Substitute.For<IUnitOfWork>();
        _service = new POIDomainService(_repository,_contract);
        _handler = new UpdatePoiCommandHandler(_logger,_service,_contract, _unitOfWork,_repository
        );
        
    }

    [Fact]
    public async Task UpdatePoiCommandHandler_Success_Case()
    {
        var mockPrevPoiCreation = await PoiHelperTestMethods.MockPrevPoiCreation("TestName","Description" ,  new CoordinateDto(42,42));
        //     "TestName",
        //     "Description",
        //     42,
        //     42

        var updatePoiCommand = new UpdatePoiCommand
        (
               new Guid("cc4eab60-5291-466c-aba1-e0a7b31e39bc"),"UpdatedTestName","UpdatedTestDescription",new CoordinateDto(42,45),POIStatus.ACTIVE
        );
        var latitude = Latitude.Create(updatePoiCommand.CoordinateDto.Latitude);
        var lon = Longitude.Create(updatePoiCommand.CoordinateDto.Longitude);
        _contract.CreateWktStringFromLonLat(latitude.Value,lon.Value).Returns(Result<string>.Success($"POINT ({latitude.Value} {lon.Value})"));
        _repository.FindPointByIdAsync(Arg.Any<PointID>()).Returns(mockPrevPoiCreation);
        var response = await _handler.Handle(updatePoiCommand);
        if (response.IsFailure)
        {
            _testOutputHelper.WriteLine(response.Error.Code);
            _testOutputHelper.WriteLine(response.Error.Desc);
            return;
        }
        
        _logger.LogInformation("Created Successfully");
        _testOutputHelper.WriteLine(response.Value.NewDesc);
        _testOutputHelper.WriteLine(response.Value.NewName);
        _testOutputHelper.WriteLine(response.Value.NewWKT);
    }
    [Fact]
    public async Task UpdatePoiCommandHandler_NullName_Failure_Case()
    {
        var mockPrevPoiCreation = await PoiHelperTestMethods.MockPrevPoiCreation("PrevName","Description",new CoordinateDto(42,38));
        //     "TestName",
        //     "Description",
        //     42,
        //     42

        var updatePoiCommand = new UpdatePoiCommand
        (
                mockPrevPoiCreation.Id.Value,"","UpdatedTestDescription",new CoordinateDto(41,25),POIStatus.ACTIVE
        );
        var latitude = Latitude.Create(updatePoiCommand.CoordinateDto.Latitude);
        var lon = Longitude.Create(updatePoiCommand.CoordinateDto.Longitude);
        _contract.CreateWktStringFromLonLat(latitude.Value,lon.Value).Returns(Result<string>.Success($"POINT ({latitude.Value} {lon.Value})"));
        _repository.FindPointByIdAsync(Arg.Any<PointID>()).Returns(mockPrevPoiCreation);
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
        var mockPrevPoiCreation = await PoiHelperTestMethods.MockPrevPoiCreation("PrevName","Description",new CoordinateDto(42,42));
        //     "TestName",
        //     "Description",
        //     42,
        //     42

        var updatePoiCommand = new UpdatePoiCommand
        (
                mockPrevPoiCreation.Id.Value,"PrevName","Description",new CoordinateDto(42,42),POIStatus.ACTIVE
        );
        var latitude = Latitude.Create(updatePoiCommand.CoordinateDto.Latitude);
        var lon = Longitude.Create(updatePoiCommand.CoordinateDto.Longitude);
        _contract.CreateWktStringFromLonLat(latitude.Value,lon.Value).Returns(Result<string>.Success($"POINT ({latitude.Value} {lon.Value})"));
        
        _repository.FindPointByIdAsync(Arg.Any<PointID>(),Arg.Any<bool>(),Arg.Any<CancellationToken>() ).Returns(mockPrevPoiCreation);
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
        var mockPrevPoiCreation = await PoiHelperTestMethods.MockPrevPoiCreation("PrevName","Description",new CoordinateDto(42,42));
        //     "TestName",
        //     "Description",
        //     42,
        //     42

        var updatePoiCommand = new UpdatePoiCommand
        (
                mockPrevPoiCreation.Id.Value,"PrevName","Description",new CoordinateDto(42,42),POIStatus.ACTIVE
        );
        var latitude = Latitude.Create(updatePoiCommand.CoordinateDto.Latitude);
        var lon = Longitude.Create(updatePoiCommand.CoordinateDto.Longitude);
        _contract.CreateWktStringFromLonLat(latitude.Value,lon.Value).Returns(Result<string>.Success($"POINT ({latitude.Value} {lon.Value})"));
        
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