using FluentAssertions;

using Gis.Application.Dtos;
using Gis.Application.Features.Poi.Queries.Distance.Wkt.FindNearestPoiByWkt;

using gis.Domain.Aggregates;
using gis.Domain.Contracts;
using gis.Domain.Entities.WKT;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern.Errors;

using Gis.Infrastructure.Topology_Services;

using Microsoft.Extensions.Logging;

using NSubstitute;

using Tests.HelperMethods.Poi;

namespace Tests.UnitTests.Application.Features.Poi.Queries.FindClosestPoiByWkt;

public class FindClosestPoiByWktHandlerTests
{
    private readonly IPointRepository _repository;
    private readonly ILogger<FindClosestPoiByWktHandler> _logger;
    private readonly FindClosestPoiByWktHandler _handler;
    private readonly ITopologySuiteWKTContract _contract;

    public FindClosestPoiByWktHandlerTests()
    {
        _repository = Substitute.For<IPointRepository>();
        _contract = Substitute.For<ITopologySuiteWKTContract>();
        _logger = Substitute.For<ILogger<FindClosestPoiByWktHandler>>();
        
        _handler = new FindClosestPoiByWktHandler(_repository, _logger, new TopologySuiteWktContractImpl());
        
    }

    [Fact]
    public async Task FIND_CLOSEST_POI_BY_WKT_NEIGHBOR_NOT_EXIST()
    {
        _repository.FindClosestPoiByWktAsync(Arg.Any<WellKnownText>()).Returns(new ValueTuple<POIAggregate, double>());

        var mock = new FindClosestPoiByWktQuery("POINT(45 22)");

        var handle = await _handler.Handle(mock);
        handle.Error.Should().Be(RepositoryErrors.NO_NEIGHBOR_FOUND);
    }
    [Fact]
    public async Task FIND_CLOSEST_POI_BY_WKT_NEIGHBOR_EXISTS()
    {

        var mockPoi = await PoiHelperTestMethods.MockPoiCreation("mockPoi", "MockDesc", new CoordinateDto(44, 35));
        _repository.FindClosestPoiByWktAsync(Arg.Any<WellKnownText>()).Returns((mockPoi,55.0));

        var mock = new FindClosestPoiByWktQuery("POINT(45 22)");
        var expectedWKT = WellKnownText.Create("POINT(45 22)");
        var resResponse = FindClosestPoiByWktResponse.CreateFromAgg(mockPoi,55.0);
        var handle = await _handler.Handle(mock);
        await _repository.Received(1).FindClosestPoiByWktAsync(expectedWKT.Value);
        
        handle.Value.Should().Be(resResponse);
    }    [Fact]
    public async Task FIND_CLOSEST_POI_BY_WKT_IS_INVALID()
    {

        var mockPoi = await PoiHelperTestMethods.MockPoiCreation("mockPoi", "MockDesc", new CoordinateDto(44, 35));
              var mock = new FindClosestPoiByWktQuery("POINT(45 22}");
        var handle = await _handler.Handle(mock);
        handle.Error.Should().Be(DomainErrors.POIErrors.WKT.INVALID_WKT_FORMAT);
    }
}