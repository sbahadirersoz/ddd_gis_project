using FluentAssertions;

using Gis.Application.Dtos;
using Gis.Application.Features.Poi.Queries.FindNearestPoi;

using gis.Domain.Aggregates;
using gis.Domain.Entities.IDs;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern.Errors;

using Microsoft.Extensions.Logging;

using NSubstitute;
using NSubstitute.ReturnsExtensions;

using Tests.HelperMethods.Poi;

namespace Tests.UnitTests.Application.Features.Poi.Queries.Distances.FindClosestByIdQuery;

public class FindClosestByIdQueryHandlerTests
{
    
    private readonly ILogger<FindClosestByIdQueryHandler> _logger;
    private readonly IPointRepository _repository;
    private readonly FindClosestByIdQueryHandler _handler;

    public FindClosestByIdQueryHandlerTests()
    {
        _logger = Substitute.For<ILogger<FindClosestByIdQueryHandler>>();
        _repository = Substitute.For<IPointRepository>();
        _handler = new FindClosestByIdQueryHandler(_logger, _repository);
    }

    [Fact]
    public async Task FIND_CLOSEST_BY_ID_POI_NOT_EXISTS()
    {
        _repository.FindPointByIdAsync(Arg.Any<PointID>()).ReturnsNull();
        var mockQuery = new Gis.Application.Features.Poi.Queries.FindNearestPoi.FindClosestByIdQuery(Guid.NewGuid());


        var handle = await _handler.Handle(mockQuery);
        handle.Error.Should().Be(RepositoryErrors.ENTITY_NOT_FOUND);
    }

    [Fact]
    public async Task FIND_CLOSEST_BY_ID_NEIGHBOR_NOT_EXISTS()
    {
        var createPoi = await PoiHelperTestMethods.MockPoiCreation("poiEntity", "desc", new CoordinateDto(44, 32));
        _repository.FindPointByIdAsync(Arg.Any<PointID>()).Returns(createPoi);
        _repository.FindClosestEntityByGivenIdAsync(Arg.Any<PointID>()).ReturnsNull();
        
        var mockQuery = new Gis.Application.Features.Poi.Queries.FindNearestPoi.FindClosestByIdQuery(Guid.NewGuid());


        var handle = await _handler.Handle(mockQuery);
        handle.Error.Should().Be(RepositoryErrors.NO_NEIGHBOR_FOUND);
    }

    [Fact]
    public async Task FIND_CLOSEST_BY_ID_SUCCESS()
    {
        var createPoi = await PoiHelperTestMethods.MockPoiCreation("source", "desc", new CoordinateDto(44, 32));
        var poiNeighbor = await PoiHelperTestMethods.MockPoiCreation("poiNeighbor", "neighbor", new CoordinateDto(44, 32));
        _repository.FindPointByIdAsync(Arg.Any<PointID>()).Returns(createPoi);
        (POIAggregate poiNeighbor, double) closestResult = (poiNeighbor, 40.2);
        _repository.FindClosestEntityByGivenIdAsync(Arg.Any<PointID>())
            .Returns(closestResult);
        
        var mockQuery = new Gis.Application.Features.Poi.Queries.FindNearestPoi.FindClosestByIdQuery(Guid.NewGuid());


        var handle = await _handler.Handle(mockQuery);
        var closestToResponse =
            FindClosestByIdQueryResponse.FromAggregate(closestResult.poiNeighbor, closestResult.Item2);
        handle.IsSuccess.Should().Be(true);
        handle.Value.Should().Be(closestToResponse);

    }

}