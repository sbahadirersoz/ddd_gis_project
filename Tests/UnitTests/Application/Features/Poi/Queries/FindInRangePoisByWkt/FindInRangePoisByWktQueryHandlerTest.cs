using FluentAssertions;

using Gis.Application.Dtos;
using Gis.Application.Features.Poi.Queries.Distance.Wkt.FindInRangePoisByWkt;
using Gis.Application.Features.Poi.Queries.Distance.Wkt.FindNearestPoiByWkt;

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

namespace Tests.UnitTests.Application.Features.Poi.Queries.FindInRangePoisByWkt;

public class FindInRangePoisByWktQueryHandlerTest
{
    
    private readonly IPointRepository _repository;
    private readonly ILogger<FindInRangePoisByWktHandler> _logger;
    private readonly FindInRangePoisByWktHandler _handler;
    private readonly ITopologySuiteWKTContract _contract;

    public FindInRangePoisByWktQueryHandlerTest()
    {
        _repository = Substitute.For<IPointRepository>();
        _contract = Substitute.For<ITopologySuiteWKTContract>();
        _logger = Substitute.For<ILogger<FindInRangePoisByWktHandler>>();
        _handler = new FindInRangePoisByWktHandler(_logger,_repository,  new TopologySuiteWktContractImpl());
        
    }
    [Fact]
    public async Task FIND_INRANGE_POIS_BY_WKT_NEIGHBOR_NOT_EXIST()
    {
        _repository.FindInRangePoisByWktAsync(Arg.Any<WellKnownText>(),Arg.Any<double>()).Returns(new  List<(POIAggregate agg, double distance)>());

        var mock = new FindInRangePoisByWktQuery("POINT(45 22)",600);
        var expectedWKT = WellKnownText.Create("POINT(45 22)").Value;

        var handle = await _handler.Handle(mock);
       await  _repository.Received(1).FindInRangePoisByWktAsync(expectedWKT, mock.distance);
        handle.Value.Should().BeEmpty();
    }
    [Fact]
    public async Task FIND_INRANGE_POI_BY_WKT_NEIGHBOR_EXISTS()
    {
        var list = await PoiHelperTestMethods.mockListWithDouble();
        _repository.FindInRangePoisByWktAsync(Arg.Any<WellKnownText>(),Arg.Any<double>()).Returns(list);

        var mock = new FindInRangePoisByWktQuery("POINT(45 22)",600);
        var expectedWKT = WellKnownText.Create("POINT(45 22)").Value;


        var resList = list.ConvertAll(x => FindInRangePoisByWktResponse.FromAgg(x.Item1, x.Item2));

        var handle = await _handler.Handle(mock);
       await  _repository.Received(1).FindInRangePoisByWktAsync(expectedWKT, mock.distance);
        
        handle.Value.Should().BeEquivalentTo(resList, o => o.WithStrictOrdering());

    }[Fact]
    public async Task FIND_INRANGE_POI_BY_WKT_IS_INVALID()
    {

        var mockPoi = await PoiHelperTestMethods.MockPoiCreation("mockPoi", "MockDesc", new CoordinateDto(44, 35));
        // _contract.ParsePointWkt(Arg.Any<string>())
        //     .Returns(Result<WellKnownText>.Failure((DomainErrors.POIErrors.WKT.INVALID_WKT_FORMAT)));
        var mock = new FindInRangePoisByWktQuery("POINT(45 22}",800);

        var handle = await _handler.Handle(mock);
        handle.Error.Should().Be(DomainErrors.POIErrors.WKT.INVALID_WKT_FORMAT);
    }

}