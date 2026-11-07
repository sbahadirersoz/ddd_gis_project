using System.ComponentModel;

using FluentAssertions;

using Gis.Application.Dtos;
using Gis.Application.Features.Poi.Queries.Distance.FindInRangePoisByName;
using Gis.Application.Features.Poi.Queries.Distance.FindNearestCountPoiByName;

using gis.Domain.Aggregates;
using gis.Domain.Entities.Information;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;

using Gis.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

using NSubstitute;
using NSubstitute.ReturnsExtensions;

using Tests.HelperMethods.Poi;

namespace Tests.UnitTests.Application.Features.Poi.Queries.FindInRangePoisByName;

public class FindInRangePoisByNameQueryHandlerTest
{
    private readonly ILogger<FindInRangePoisByNameQueryHandler> _logger;
    private readonly IPointRepository _repository;
    private readonly FindInRangePoisByNameQueryHandler _handler;

    public FindInRangePoisByNameQueryHandlerTest()
    {
        _logger = Substitute.For<ILogger<FindInRangePoisByNameQueryHandler>>();
        _repository = Substitute.For<IPointRepository>();
        _handler = new FindInRangePoisByNameQueryHandler(_logger,_repository);
    }

    [Fact]
    public async Task  FIND_IN_RANGE_POI_NULL()
    {
        _repository.IsPointNameExistsAsync(Arg.Any<PointName>()).Returns(false);
        var mockQuery = new FindInRangePoisByNameQuery("Name",455);
        var handle = await _handler.Handle(mockQuery);
        handle.Error.Should().Be(RepositoryErrors.ENTITY_NOT_FOUND);
    }
    [Fact]
    public async Task  FIND_IN_RANGE_NEIGHBOR_NULL()
    {
        _repository.IsPointNameExistsAsync(Arg.Any<PointName>()).Returns(true);
        _repository.FindInRangePoisByNameAsync(Arg.Any<PointName>(),Arg.Any<double>()).Returns(new List<(POIAggregate agg, double distance)>());
        var mockQuery = new FindInRangePoisByNameQuery("Name",455);
        var handle = await _handler.Handle(mockQuery);
        handle.Value.Should().BeEmpty();
    }
    [Fact]
    public async Task  FIND_IN_RANGE_SUCCESS()
    {
        _repository.IsPointNameExistsAsync(Arg.Any<PointName>()).Returns(true);
        var list = await PoiHelperTestMethods.mockListWithDouble();
        
        
        _repository.FindInRangePoisByNameAsync(Arg.Any<PointName>(),Arg.Any<double>()).Returns(list);
        var mockListResult = list.ConvertAll(x => FindInRangePoisByNameQueryResponse.FromAgg(x.Item1, x.Item2));
        var mockQuery = new FindInRangePoisByNameQuery("Name",455);
        
        var handle = await _handler.Handle(mockQuery);
        handle.Value.Should().BeEquivalentTo(mockListResult, o => o.WithStrictOrdering());
        
    }


}