using System.Text.Json;
using gis.ApplicationLayer.Dtos;
using gis.ApplicationLayer.Features.Poi.Queries.Distance.FindNearestCountPoiByName;
using gis.ApplicationLayer.Features.Poi.Queries.FindPoisInGivenRange;
using gis.Domain.Aggregates;
using gis.Domain.Entities.Information;
using gis.Domain.Repositories;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Tests.HelperMethods.Poi;
using Xunit.Abstractions;

namespace Tests.UnitTests.Application.Features.Poi.Queries.FindNearestCountPoiByName;

public class FindNearestCountPoiByNameHandlerTests
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly ILogger<FindMostClosesCountByNameQueryHandler> _logger;
    private readonly FindMostClosesCountByNameQueryHandler _handler;
    private readonly IPointRepository _pointRepository;

    public FindNearestCountPoiByNameHandlerTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _pointRepository = Substitute.For<IPointRepository>();
        _logger = Substitute.For<ILogger<FindMostClosesCountByNameQueryHandler>>();
        _handler = new FindMostClosesCountByNameQueryHandler(_pointRepository ,_logger);
        
    }

    [Fact]
    public async Task Success_Case()
    {
        var unit1 =  await PoiHelperTestMethods.MockPrevPoiCreation("unit1","unit1desc",new CoordinateDto(41,42));

        var list = await PoiHelperTestMethods.mockList();
        var mockListWithDistance = new List<(POIAggregate x, double s)>();
        mockListWithDistance.Add((unit1, 520.1));
        var mockQuery = new FindMostClosesCountByNameQuery(PointName.FromString("TestFirst").Value.Value,3);
        _pointRepository.FindMostClosesCountByPoiName(Arg.Any<PointName>(),Arg.Any<int>()).Returns(mockListWithDistance);
        var result =await _handler.Handle(mockQuery);
        var str = JsonSerializer.Serialize(result);
        _testOutputHelper.WriteLine(str);
    }
}