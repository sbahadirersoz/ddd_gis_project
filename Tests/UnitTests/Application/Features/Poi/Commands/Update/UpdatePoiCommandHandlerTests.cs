using FluentAssertions;

using Gis.Application.Common;
using Gis.Application.Dtos;
using Gis.Application.Features.Poi.Commands;
using Gis.Application.Features.Poi.Commands.Update;

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
        _handler = new UpdatePoiCommandHandler(_logger,_service, _unitOfWork,_repository
        );
        
    }
    [Fact]
    public async Task UpdatePoiCommandHandler_Success_Case()
    {
        var mockPrevPoiCreation = await PoiHelperTestMethods.MockPoiCreation("TestName","Description" ,  new CoordinateDto(42,42));
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
        _contract.CreateWktStringFromLonLat(lon.Value,latitude.Value).Returns(Result<string>.Success($"POINT ({lon.Value.Value} {latitude.Value.Value})"));
        _repository.FindPointByIdAsync(Arg.Any<PointID>(),Arg.Is(false),Arg.Any<CancellationToken>()).Returns(mockPrevPoiCreation);
        var response = await _handler.Handle(updatePoiCommand);
        
        
        response.IsSuccess.Should().BeTrue();
        response.Value.Should().NotBeNull();
        response.Value.NewName.Should().Be("UpdatedTestName");
        response.Value.NewDesc.Should().Be("UpdatedTestDescription");
        response.Value.NewWKT.Should().Be("POINT (42 45)");
        _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        _repository.Received(1).Update(Arg.Any<POIAggregate>());
    }
    [Fact]
    public async Task UpdatePoiCommandHandler_EmptyName_Failure_Case()
    {
        var mockPrevPoiCreation = await PoiHelperTestMethods.MockPoiCreation("PrevName","Description",new CoordinateDto(42,38));


        var updatePoiCommand = new UpdatePoiCommand
        (
                mockPrevPoiCreation.Id.Value,"","UpdatedTestDescription",new CoordinateDto(41,25),POIStatus.ACTIVE
        );
         var response = await _handler.Handle(updatePoiCommand);
        response.IsFailure.Should().BeTrue();
        response.Error.Should().Be(DomainErrors.POIErrors.PointName.BAD_CREDENTIALS_FOR_POINT_NAME);
        _repository.DidNotReceive().FindPointByIdAsync(Arg.Any<PointID>(),Arg.Any<bool>(),Arg.Any<CancellationToken>());
        _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());

    }

    [Fact]
    public async Task? UpdatePoiCommandHandler_NothingChanged_Failure_Case()
    {
        var mockPrevPoiCreation = await PoiHelperTestMethods.MockPoiCreation("PrevName","Description",new CoordinateDto(42,42));
  

        var updatePoiCommand = new UpdatePoiCommand
        (
                mockPrevPoiCreation.Id.Value,"PrevName","Description",new CoordinateDto(42,42),POIStatus.ACTIVE
        );
        _repository.FindPointByIdAsync(Arg.Any<PointID>(),Arg.Any<bool>(),Arg.Any<CancellationToken>() ).Returns(mockPrevPoiCreation);
       var response = await _handler.Handle(updatePoiCommand);
       _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
        response.Error.Should().Be(DomainServiceErrors.SAME_CREDENTIALS_FOR_UPDATING);
        
        
    }  [Fact]
    public async Task? UpdatePoiCommandHandler_PoiNotFound_Failure_Case()
    {
        var mockPrevPoiCreation = await PoiHelperTestMethods.MockPoiCreation("PrevName","Description",new CoordinateDto(42,42));

        var updatePoiCommand = new UpdatePoiCommand
        (
                mockPrevPoiCreation.Id.Value,"PrevName","Description",new CoordinateDto(42,42),POIStatus.ACTIVE
        );
        var response = await _handler.Handle(updatePoiCommand);
        response.Error.Should().Be(RepositoryErrors.ENTITY_NOT_FOUND);
    }

    [Fact]
    public async Task UpdatePoiCommandHandler_OnlyDesc_UpdatesDescAndKeepsOthers()
    {
        var prev = await PoiHelperTestMethods.MockPoiCreation("PrevName", "PrevDesc", new CoordinateDto(42, 35));
        _repository.FindPointByIdAsync(Arg.Any<PointID>(), Arg.Any<bool>(), Arg.Any<CancellationToken>()).Returns(prev);

        var response = await _handler.Handle(new UpdatePoiCommand(prev.Id.Value, PointDesc: "NewDesc"));

        response.IsSuccess.Should().BeTrue();
        response.Value.NewDesc.Should().Be("NewDesc");
        response.Value.NewName.Should().Be("PrevName");
        response.Value.NewWKT.Should().Be("POINT (42 35)");
    }

    [Fact]
    public async Task UpdatePoiCommandHandler_OnlyName_UpdatesNameAndKeepsOthers()
    {
        var prev = await PoiHelperTestMethods.MockPoiCreation("PrevName", "PrevDesc", new CoordinateDto(42, 35));
        _repository.FindPointByIdAsync(Arg.Any<PointID>(), Arg.Any<bool>(), Arg.Any<CancellationToken>()).Returns(prev);

        var response = await _handler.Handle(new UpdatePoiCommand(prev.Id.Value, PoiName: "NewName"));

        response.IsSuccess.Should().BeTrue();
        response.Value.NewName.Should().Be("NewName");
        response.Value.NewDesc.Should().Be("PrevDesc");
        response.Value.NewWKT.Should().Be("POINT (42 35)");
    }
}