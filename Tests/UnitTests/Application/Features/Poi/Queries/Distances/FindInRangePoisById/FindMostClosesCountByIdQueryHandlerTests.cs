using System.Globalization;
using System.Runtime.InteropServices.JavaScript;
using gis.ApplicationLayer.Dtos;
using gis.ApplicationLayer.Features.Poi.Queries.FindPoiById;
using gis.ApplicationLayer.Features.Poi.Queries.FindPoisInGivenRange;
using gis.Domain.Aggregates;
using gis.Domain.Common;
using gis.Domain.Entities.IDs;
using gis.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NSubstitute;
using Tests.HelperMethods.Poi;
using Xunit.Abstractions;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Tests.UnitTests.Application.Features.Poi.Queries.FindInRangePoisById;

public class FindMostClosesCountByIdQueryHandlerTests
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly ILogger<FindMostClosesCountByIdQueryHandler> _logger;
    private readonly FindMostClosesCountByIdQueryHandler _handler;
    private readonly IPointRepository _pointRepository;

    public FindMostClosesCountByIdQueryHandlerTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _logger = Substitute.For<ILogger<FindMostClosesCountByIdQueryHandler>>();
        _pointRepository =  Substitute.For<IPointRepository>();
        _handler = new FindMostClosesCountByIdQueryHandler( _logger,_pointRepository);
    }



    public async Task<List<(POIAggregate,double)>> mockList()
    {
        var unit1 =  (await PoiHelperTestMethods.MockPrevPoiCreation("unit1","unit1desc",new CoordinateDto(42,35)),4250D);
        var unit2 = (await PoiHelperTestMethods.MockPrevPoiCreation("unit2","unit2desc",new CoordinateDto(42,35)),35D);
        var unit3 = (await PoiHelperTestMethods.MockPrevPoiCreation("unit3","unit3desc",new CoordinateDto(42,35)),1555D);
        List<(POIAggregate,double)> l = new List<(POIAggregate,double)>();
        l.Add(unit1);
        l.Add(unit2);
        l.Add(unit3);
        return l;
    }
    
    [Fact]
    public async void TestSuccess()
    {
        var list =   await mockList();
        _pointRepository.FindMostClosesCountByIdAsync(Arg.Any<PointID>(), Arg.Any<int>()).Returns(list);
        var mockQuery = new FindMostClosesCountByIdQuery(PointID.New().Value,3);
        var result = await _handler.Handle(mockQuery);
        var serialize = JsonSerializer.Serialize(result);
        _testOutputHelper.WriteLine(serialize);
    }
}