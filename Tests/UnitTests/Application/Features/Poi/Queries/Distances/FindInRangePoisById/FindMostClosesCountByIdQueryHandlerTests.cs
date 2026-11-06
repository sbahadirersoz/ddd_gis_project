using FluentAssertions;

using Gis.Application.Dtos;
using Gis.Application.Features.Poi.Queries.Distance.Id.FindInRangePoisById;
using Gis.Application.Features.Poi.Queries.FindPoisInGivenRange;

using gis.Domain.Aggregates;
using gis.Domain.Contracts;
using gis.Domain.Entities.IDs;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern.Errors;

using Microsoft.Extensions.Logging;

using NSubstitute;
using NSubstitute.ReturnsExtensions;

using Tests.HelperMethods.Poi;

using Xunit.Abstractions;

namespace Tests.UnitTests.Application.Features.Poi.Queries.FindInRangePoisById;

public class FindMostClosesCountByIdQueryHandlerTests
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly ILogger<FindMostClosesCountByIdQueryHandler> _logger;
    private readonly FindMostClosesCountByIdQueryHandler _handler;
    private readonly IPointRepository _pointRepository;
    private readonly ITopologySuiteWKTContract _contract;

    public FindMostClosesCountByIdQueryHandlerTests(ITestOutputHelper testOutputHelper)
    {
        _contract = Substitute.For<ITopologySuiteWKTContract>();
        _testOutputHelper = testOutputHelper;
        _logger = Substitute.For<ILogger<FindMostClosesCountByIdQueryHandler>>();
        _pointRepository = Substitute.For<IPointRepository>();
        _handler = new FindMostClosesCountByIdQueryHandler(_logger, _pointRepository);
    }


    public async Task<List<(POIAggregate, double)>> mockList()
    {
        var unit1 = (await PoiHelperTestMethods.MockPoiCreation("unit1", "unit1desc", new CoordinateDto(42, 35)),
            4250D);
        var unit2 = (await PoiHelperTestMethods.MockPoiCreation("unit2", "unit2desc", new CoordinateDto(42, 35)),
            35D);
        var unit3 = (await PoiHelperTestMethods.MockPoiCreation("unit3", "unit3desc", new CoordinateDto(42, 35)),
            1555D);
        List<(POIAggregate, double)> l = new List<(POIAggregate, double)>();
        l.Add(unit1);
        l.Add(unit2);
        l.Add(unit3);
        return l;
    }

    [Fact]
    public async Task FIND_MOST_CLOSES_COUNT_BY_ID_POI_NO_FOUND()
    {
        _pointRepository.FindPointByIdAsync(Arg.Any<PointID>()).ReturnsNull();
        var mockQuery = new FindMostClosesCountByIdQuery(PointID.New().Value, 3);
        

        var result = await _handler.Handle(mockQuery);

        result.IsFailure.Should().Be(true);
        result.Error.Should().Be(RepositoryErrors.ENTITY_NOT_FOUND);
    }

    [Fact]
    public async Task FIND_MOST_CLOSES_COUNT_BY_ID_NEIGHBOR_NO_FOUND()
    {
        var createdPoi =  await PoiHelperTestMethods.MockPoiCreation("MockPoi","Descblabla",new CoordinateDto(45,32));
        _pointRepository.FindPointByIdAsync(Arg.Any<PointID>()).Returns(createdPoi);
        _pointRepository.FindMostClosesCountByIdAsync(Arg.Any<PointID>(), Arg.Any<int>()).Returns(new List<(POIAggregate entity, double distance)>());
        var mockQuery = new FindMostClosesCountByIdQuery(PointID.New().Value, 3);

        var result = await _handler.Handle(mockQuery);
        result.Value.Should().BeEquivalentTo(new List<FindMostClosesCountByIdQueryResponse>());

    }

    [Fact]
    public async Task FIND_MOST_CLOSES_COUNT_BY_ID_SUCCESS()
    {
        var list = await mockList();
        var createdPoi =  await PoiHelperTestMethods.MockPoiCreation("MockPoi","Descblabla",new CoordinateDto(45,32));
        
        _pointRepository.FindPointByIdAsync(Arg.Any<PointID>()).Returns(createdPoi);
        _pointRepository.FindMostClosesCountByIdAsync(Arg.Any<PointID>(), Arg.Any<int>()).Returns(list);
        
        var mockQuery = new FindMostClosesCountByIdQuery(PointID.New().Value, 3);
        var res = list.ConvertAll(x => FindMostClosesCountByIdQueryResponse.CreateFromAgg(x.Item1, x.Item2));

        var result = await _handler.Handle(mockQuery);
        result.Value[0].Should().BeEquivalentTo(res[0]);
        result.Value[1].Should().BeEquivalentTo(res[1]);
        result.Value[2].Should().BeEquivalentTo(res[2]);
    }
}