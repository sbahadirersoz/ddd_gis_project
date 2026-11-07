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
        var mockList = await PoiHelperTestMethods.mockListWithDouble();
        _repository.FindInRangePoisByLonLatAsync(Arg.Any<Longitude>(), Arg.Any<Latitude>(), Arg.Any<double>())
            .Returns(mockList);
        var mockQuery = new FindInRangePoisByLonLatQuery(34, 32, 555);
        var resultList = mockList.ConvertAll(x => FindInRangePoisByLonLatResponse.FromAgg(x.Item1, x.Item2));
        var handler = await _handler.Handle(mockQuery);
        handler.Value.Should().BeEquivalentTo(resultList, o => o.WithStrictOrdering());
    }





}