using FluentAssertions;

using Gis.Application.Common;
using Gis.Application.Dtos;
using Gis.Application.Features.Poi.Queries.Distance.FindNearestPoiByName;
using Gis.Application.Features.Poi.Queries.FindPoiById;

using gis.Domain.Aggregates;
using gis.Domain.Entities.Information;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;

using Microsoft.Extensions.Logging;

using NSubstitute;
using NSubstitute.Core;
using NSubstitute.ReturnsExtensions;

using Tests.HelperMethods.Poi;

using Xunit.Abstractions;

namespace Tests.UnitTests.Application.Features.Poi.Queries.FindClosestPoiByName;

public class FindClosestPoiByNameHandlerTest
{
    private readonly ITestOutputHelper _testOutputHelper;

    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<FindClosestPoiByNameQueryHandler> _logger;
    private readonly FindClosestPoiByNameQueryHandler _handler;
    private readonly IPointRepository _pointRepository;

    public FindClosestPoiByNameHandlerTest(ITestOutputHelper testOutputHelper)
    {
        this._testOutputHelper = testOutputHelper;
        _unitOfWork  = Substitute.For<IUnitOfWork>();
        _logger = Substitute.For<ILogger<FindClosestPoiByNameQueryHandler>>();
        _pointRepository =  Substitute.For<IPointRepository>();
        _handler = new FindClosestPoiByNameQueryHandler( _logger,_pointRepository);
    }

    [Fact]
    public async Task POI_NOT_FOUND()
    {
        _pointRepository.FindPointByPointNameAsync(Arg.Any<PointName>()).ReturnsNull();
        
        var res = await _handler.Handle(new FindClosestPoiByNameQuery("TestName"));
        
        res.IsFailure.Should().Be(true);
        res.Error.Should().Be(RepositoryErrors.ENTITY_NOT_FOUND);
        
    }
   
    [Fact]
    public async Task POI_NEIGHBOR_NOT_FOUND()
    {  
         var findPointByPointNameAsyncFoundResultPoi = await  PoiHelperTestMethods.MockPoiCreation("MockName","MockDescCreate",new CoordinateDto(42,45));
        _pointRepository.FindPointByPointNameAsync(Arg.Any<PointName>()).Returns(findPointByPointNameAsyncFoundResultPoi);
        _pointRepository.FindClosestPoiByName(Arg.Any<PointName>()).ReturnsNull();
        
        var res = await _handler.Handle(new FindClosestPoiByNameQuery("TestName"));
        
        res.IsFailure.Should().Be(true);
        res.Error.Should().Be(RepositoryErrors.NO_NEIGHBOR_FOUND);
        
    }
    [Fact]
       public async Task POI_NEIGHBOR_FOUND_SUCCES()
    {  
         var findPointByPointNameAsyncResult = await  PoiHelperTestMethods.MockPoiCreation("MockName","MockDescCreate",new CoordinateDto(42,45));
         var findNearestPointByNameAsyncResult = await  PoiHelperTestMethods.MockPoiCreation("NeighBor","MockDescCreate",new CoordinateDto(42,45));
         
        _pointRepository.FindPointByPointNameAsync(Arg.Any<PointName>()).Returns(findPointByPointNameAsyncResult);
        var resultTuple = new ValueTuple<POIAggregate, double>(findNearestPointByNameAsyncResult, 552);
        _pointRepository.FindClosestPoiByName(Arg.Any<PointName>()).Returns(resultTuple);
        
        var res = await _handler.Handle(new FindClosestPoiByNameQuery("TestName"));
        
        res.IsFailure.Should().Be(false);
        res.Value.name.Should().Be(findNearestPointByNameAsyncResult.PointName.Value);
        res.Value.distanceByMeters.Should().Be(resultTuple.Item2);
    }
   
}