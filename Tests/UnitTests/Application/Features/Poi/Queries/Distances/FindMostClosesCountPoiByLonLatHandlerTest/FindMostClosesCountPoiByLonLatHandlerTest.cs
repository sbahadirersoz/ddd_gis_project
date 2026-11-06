using FluentAssertions;

using Gis.Application.Dtos;
using Gis.Application.Features.Poi.Queries.Distance.LonLat.FindClosestPoiByLonLat;
using Gis.Application.Features.Poi.Queries.Distance.LonLat.FindInRangePoisByLonLat;
using Gis.Application.Features.Poi.Queries.Distance.LonLat.FindMostClosesCountPoiByLonLat;

using gis.Domain.Aggregates;
using gis.Domain.Entities.Coord;
using gis.Domain.Repositories;

using Microsoft.Extensions.Logging;

using NSubstitute;

using Tests.HelperMethods.Poi;

namespace Tests.UnitTests.Application.Features.Poi.Queries.Distances.FindMostClosesCountPoiByLonLatHandlerTest;

public class FindMostClosesCountPoiByLonLatHandlerTest
{
    
    private readonly IPointRepository _repository;
    private readonly ILogger<FindMostClosesCountPoiByLonLatHandler> _logger;
    private readonly FindMostClosesCountPoiByLonLatHandler _handler;

    public FindMostClosesCountPoiByLonLatHandlerTest()
    {
        _repository = Substitute.For<IPointRepository>();
        _logger =  Substitute.For<ILogger<FindMostClosesCountPoiByLonLatHandler>>();
        _handler =  new FindMostClosesCountPoiByLonLatHandler(_repository,_logger);

    }

    
    [Fact]
    public async Task FIND_MOST_CLOSES_POIS_WITH_COUNT_RETURN_EMPTY()
    {
        _repository.FindMostClosesCountByLonLatAsync(Arg.Any<Longitude>(), Arg.Any<Latitude>(), Arg.Any<int>())
            .Returns(new List<(POIAggregate agg, double distance)>());
        var mockQuery = new FindMostClosesCountPoiByLonLatQuery(34, 32, 3);
        var handler =  await _handler.Handle(mockQuery);
        handler.Value.Should().BeEmpty();
    }

    [Fact]
    public async Task FIND_MOST_CLOSES_POIS_WITH_COUNT_RETURN_VALUE()
    {
        var mockList = await this.mockList();
        _repository.FindMostClosesCountByLonLatAsync(Arg.Any<Longitude>(), Arg.Any<Latitude>(), Arg.Any<int>())
            .Returns(mockList);
        var mockQuery = new FindMostClosesCountPoiByLonLatQuery(34, 32, 3);
        var resultList = mockList.ConvertAll(x => FindInRangePoisByLonLatResponse.FromAgg(x.Item1, x.Item2));
        var handler = await _handler.Handle(mockQuery);
        handler.Value.Should().BeEquivalentTo(resultList, o => o.WithStrictOrdering());
    }


    public async Task<List<(POIAggregate, double)>> mockList()
    {
        var unit1 = (await PoiHelperTestMethods.MockPoiCreation("unit1", "unit1desc", new CoordinateDto(42, 35)),
            4250D);
        var unit2 = (await PoiHelperTestMethods.MockPoiCreation("unit2", "unit2desc", new CoordinateDto(42, 35)),
            35D);
        var unit3 = (await PoiHelperTestMethods.MockPoiCreation("unit3", "unit3desc", new CoordinateDto(42, 35)),
            1555D);
        List<(POIAggregate, double)> l = new  ();
        l.Add(unit1);
        l.Add(unit2);
        l.Add(unit3);
        return l;
    }

}