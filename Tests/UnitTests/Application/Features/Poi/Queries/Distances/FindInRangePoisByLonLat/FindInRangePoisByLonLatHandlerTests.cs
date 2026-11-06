using FluentAssertions;

using Gis.Application.Dtos;
using Gis.Application.Features.Poi.Queries.Distance.LonLat.FindClosestPoiByLonLat;
using Gis.Application.Features.Poi.Queries.Distance.LonLat.FindInRangePoisByLonLat;

using gis.Domain.Aggregates;
using gis.Domain.Entities.Coord;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern;

using Microsoft.Extensions.Logging;

using NSubstitute;
using NSubstitute.ReturnsExtensions;

using Tests.HelperMethods.Poi;

namespace Tests.UnitTests.Application.Features.Poi.Queries.Distances.FindInRangePoisByLonLat;

public class FindInRangePoisByLonLatHandlerTests
{
    
    private readonly IPointRepository _repository;
    private readonly ILogger<FindInRangePoisByLonLatHandler> _logger;
    private readonly FindInRangePoisByLonLatHandler _handler;
    

    public FindInRangePoisByLonLatHandlerTests()
    {
        _repository = Substitute.For<IPointRepository>();
        _logger =  Substitute.For<ILogger<FindInRangePoisByLonLatHandler>>();
        _handler =  new FindInRangePoisByLonLatHandler(_repository,_logger);
    }

    [Fact]
    public async Task FIND_IN_RANGE_POIS_RETURN_EMPTY()
    {
        _repository.FindInRangePoisByLonLatAsync(Arg.Any<Longitude>(), Arg.Any<Latitude>(), Arg.Any<double>())
            .Returns(new List<(POIAggregate agg, double distance)>());
        var mockQuery = new FindInRangePoisByLonLatQuery(34, 32, 555);
        var handler =  await _handler.Handle(mockQuery);
        handler.Value.Should().BeEmpty();
    }

    [Fact]
    public async Task FIND_IN_RANGE_POIS_RETURN_VALUE()
    {
        var mockList = await this.mockList();
        _repository.FindInRangePoisByLonLatAsync(Arg.Any<Longitude>(), Arg.Any<Latitude>(), Arg.Any<double>())
            .Returns(mockList);
        var mockQuery = new FindInRangePoisByLonLatQuery(34, 32, 555);
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