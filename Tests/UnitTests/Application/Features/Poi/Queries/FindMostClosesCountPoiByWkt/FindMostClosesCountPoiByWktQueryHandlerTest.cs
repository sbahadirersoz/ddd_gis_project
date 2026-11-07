using FluentAssertions;

using Gis.Application.Dtos;
using Gis.Application.Features.Poi.Queries.Distance.FindInRangePoisByName;
using Gis.Application.Features.Poi.Queries.Distance.Wkt.FindInRangePoisByWkt;
using Gis.Application.Features.Poi.Queries.Distance.Wkt.FindMostClosesCountPoiByWkt;

using gis.Domain.Aggregates;
using gis.Domain.Contracts;
using gis.Domain.Entities.WKT;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;

using Gis.Infrastructure.Topology_Services;

using Microsoft.Extensions.Logging;

using NSubstitute;

using Tests.HelperMethods.Poi;

namespace Tests.UnitTests.Application.Features.Poi.Queries.FindMostClosesCountPoiByWkt;

public class FindMostClosesCountPoiByWktQueryHandlerTest
{
    
    
    private readonly ILogger<FindMostClosesCountPoiByWktQueryHandler> _logger;
    private readonly IPointRepository _repository;
    private readonly FindMostClosesCountPoiByWktQueryHandler _handler;
    private readonly TopologySuiteWktContractImpl _contract;
    
      public FindMostClosesCountPoiByWktQueryHandlerTest()
    {
        _repository = Substitute.For<IPointRepository>();
        _logger = Substitute.For<ILogger<FindMostClosesCountPoiByWktQueryHandler>>();
        _handler = new FindMostClosesCountPoiByWktQueryHandler(_repository,_logger,  new TopologySuiteWktContractImpl());
        
    }
      [Fact]
    public async Task FIND_MOST_CLOSES_POIS_BY_WKT_COUNT_NEIGHBOR_NOT_EXIST()
    {
        _repository.FindMostClosesCountByWktAsync(Arg.Any<WellKnownText>(),Arg.Any<int>()).Returns(new  List<(POIAggregate agg, double distance)>());

        var mock = new FindMostClosesCountPoiByWktQuery("POINT(45 22)",3);
        var expectedWKT = WellKnownText.Create("POINT(45 22)").Value;


        var handle = await _handler.Handle(mock);
        await _repository.Received(1).FindMostClosesCountByWktAsync(expectedWKT, mock.count);
        handle.Value.Should().BeEmpty();
    }
    [Fact]
    public async Task FIND_MOST_CLOSES_POI_BY_WKT_NEIGHBOR_EXISTS()
    {
        var list = await PoiHelperTestMethods.mockListWithDouble();
        _repository.FindMostClosesCountByWktAsync(Arg.Any<WellKnownText>(),Arg.Any<int>()).Returns(list);
        var mock = new FindMostClosesCountPoiByWktQuery("POINT(45 22)",6);
        var expectedWKT = WellKnownText.Create("POINT(45 22)").Value;

        var resList = list.ConvertAll(x => FindMostClosesCountPoiByWktQueryResponse.FromAgg(x.Item1, x.Item2));
        var handle = await _handler.Handle(mock);
        
        await _repository.Received(1).FindMostClosesCountByWktAsync(expectedWKT, mock.count);
        handle.Value.Should().BeEquivalentTo(resList, o => o.WithStrictOrdering());
        handle.Value[0].name.Should().Be("unit1");

    }[Fact]
    public async Task FIND_CLOSES_POI_BY_WKT_IS_INVALID()
    {

        var mockPoi = await PoiHelperTestMethods.MockPoiCreation("mockPoi", "MockDesc", new CoordinateDto(44, 35));
      
        var mock = new FindMostClosesCountPoiByWktQuery("POINT(45 22}",6);
        
        var expectedWKT = WellKnownText.Create("POINT(45 22)").Value;


        
        var handle = await _handler.Handle(mock);
        handle.Error.Should().Be(DomainErrors.POIErrors.WKT.INVALID_WKT_FORMAT);
    }
  
}