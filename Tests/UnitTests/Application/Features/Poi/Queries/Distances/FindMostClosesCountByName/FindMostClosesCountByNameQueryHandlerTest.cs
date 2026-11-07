using FluentAssertions;

using Gis.Application.Dtos;
using Gis.Application.Features.Poi.Queries.Distance.FindNearestCountPoiByName;

using gis.Domain.Aggregates;
using gis.Domain.Entities.Information;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;

using Microsoft.Extensions.Logging;

using NSubstitute;
using NSubstitute.ReturnsExtensions;

using Tests.HelperMethods.Poi;

namespace Tests.UnitTests.Application.Features.Poi.Queries.Distances.FindMostClosesCountByName;

public class FindMostClosesCountByNameQueryHandlerTest
{
    private readonly ILogger<FindMostClosesCountByNameQueryHandler> _logger;
    private readonly IPointRepository _repository;
    private readonly FindMostClosesCountByNameQueryHandler _handler;

    public FindMostClosesCountByNameQueryHandlerTest()
    {
        _logger = Substitute.For<ILogger<FindMostClosesCountByNameQueryHandler>>();
        _repository = Substitute.For<IPointRepository>();
        _handler = new FindMostClosesCountByNameQueryHandler(_repository, _logger);
    }

    [Fact]
    public async Task FIND_MOST_CLOSES_COUNT_BY_NAME_POI_NOT_FOUND()
    {
        _repository.FindPointByPointNameAsync(Arg.Any<PointName>()).ReturnsNull();

        var mockQueery = new FindMostClosesCountByNameQuery("name",4);
        var result = await _handler.Handle(mockQueery);
        result.Error.Should().Be(RepositoryErrors.ENTITY_NOT_FOUND);
    }
    [Fact]
    public async Task FIND_MOST_CLOSES_COUNT_BY_NAME_NEIGHBOR_NOT_FOUND()
    {
        var created = PoiHelperTestMethods.MockPoiCreation("mockPoi","descTest",
            new CoordinateDto(47,41));
        
        _repository.FindPointByPointNameAsync(Arg.Any<PointName>()).Returns(created);
        _repository.FindMostClosesCountByPoiName(Arg.Any<PointName>(), Arg.Any<int>())
            .Returns(new List<(POIAggregate agg, double distance)>());

        var mockQueery = new FindMostClosesCountByNameQuery("name",4);
        var result = await _handler.Handle(mockQueery);
        result.Value.Should().BeEmpty();
        
    }

    [Fact]
    public async Task FIND_MOST_CLOSES_COUNT_BY_NAME_SUCCESS()
    {
        var list = await PoiHelperTestMethods.mockListWithDouble();
        var created = PoiHelperTestMethods.MockPoiCreation("mockPoi","descTest",
            new CoordinateDto(47,41));
        
        _repository.FindPointByPointNameAsync(Arg.Any<PointName>()).Returns(created);
        _repository.FindMostClosesCountByPoiName(Arg.Any<PointName>(), Arg.Any<int>())
            .Returns(list);

        var res = list.ConvertAll(x => FindMostClosesCountByNameQueryResponse.FromAgg(x.Item1, x.Item2));
        var mockQueery = new FindMostClosesCountByNameQuery("name",4);
        var result = await _handler.Handle(mockQueery);
        result.Value.Should().BeEquivalentTo(res ,o =>o.WithStrictOrdering());
    }
    




}