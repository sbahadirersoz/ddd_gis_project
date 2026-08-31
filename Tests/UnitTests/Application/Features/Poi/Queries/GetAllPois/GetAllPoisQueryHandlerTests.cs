using System.Text.Json;
using gis.ApplicationLayer.Common;
using gis.ApplicationLayer.Features.Poi.Queries.GetAllPois;
using gis.Domain.Aggregates;
using gis.Domain.Repositories;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Tests.HelperMethods.Poi;
using Xunit.Abstractions;

namespace Tests.UnitTests.Application.Features.Poi.Queries.GetAllPois;

public class GetAllPoisQueryHandlerTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    private readonly IUnitOfWork _unitOfWork;
    private readonly IPointRepository _pointRepository;
    private readonly ILogger<GetAllPoisQueryHandler> _logger;
    private readonly GetAllPoisQueryHandler _handler;

    public GetAllPoisQueryHandlerTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _logger = Substitute.For<ILogger<GetAllPoisQueryHandler>>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _pointRepository = Substitute.For<IPointRepository>();
        _handler = new GetAllPoisQueryHandler( _logger,_pointRepository);
    }

    [Fact]
    public async Task GetAllPoisQueryHandler_SuccessCase()
    {

        var mockPrevPoiCreation = await PoiHelperTestMethods.MockPrevPoiCreation("PrevName", "Description", 42, 42);
        var mockPrevPoiCreation1 = await PoiHelperTestMethods.MockPrevPoiCreation("PrevName1", "Description", 42, 42);
        List<POIAggregate> mockList = new List<POIAggregate>();
        mockList.Add(mockPrevPoiCreation);
        mockList.Add(mockPrevPoiCreation1);
        _pointRepository.GetAllEntitiesAsync().Returns(mockList);
        var result = await _handler.Handle(new GetAllPoisQuery() );
        if (result.IsFailure)
        {
            _testOutputHelper.WriteLine(result.Error.Code);
            _testOutputHelper.WriteLine(result.Error.Desc);
            return;
        }
        string value = JsonSerializer.Serialize(result.Value);
        _testOutputHelper.WriteLine(value);

        /*
        foreach (var poiAggregate in mockList)
        {
            _testOutputHelper.WriteLine("=================================");
            _testOutputHelper.WriteLine(poiAggregate.Id.Value.ToString());
            _testOutputHelper.WriteLine(poiAggregate.PointDesc.Value);
            _testOutputHelper.WriteLine(poiAggregate.PointName.Value);
            _testOutputHelper.WriteLine(poiAggregate.Coordinates.WKT.Value);
            _testOutputHelper.WriteLine("=================================");
        }
        */


    }
 }