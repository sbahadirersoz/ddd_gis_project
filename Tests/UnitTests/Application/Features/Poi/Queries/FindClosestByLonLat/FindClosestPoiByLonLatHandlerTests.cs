using FluentAssertions;

using Gis.Application.Features.Poi.Queries.Distance.LonLat.FindClosestPoiByLonLat;

using gis.Domain.Repositories;
using gis.Domain.ResultPattern.Errors;

using Microsoft.Extensions.Logging;

using NSubstitute;

namespace Tests.UnitTests.Application.Features.Poi.Queries.FindClosestByLonLat;

public class FindClosestPoiByLonLatHandlerTests
{
    private readonly ILogger<FindClosestPoiByLonLatHandler> _logger;
    private readonly IPointRepository _repository;
    private readonly FindClosestPoiByLonLatHandler _handler;

    public FindClosestPoiByLonLatHandlerTests()
    {
        _repository = Substitute.For<IPointRepository>();
        _logger = Substitute.For<ILogger<FindClosestPoiByLonLatHandler>>();
        _handler = new FindClosestPoiByLonLatHandler(_logger, _repository);
    }

    [Fact]
    public async Task FIND_CLOSEST_POI_BY_LONLAT_RETURNS_NO_NEIGH_FOUND()
    {
        FindClosestPoiByLonLatQuery mockq = new (35,44);
        var handle =  await  _handler.Handle(mockq);
        handle.Error.Should().Be(RepositoryErrors.NO_NEIGHBOR_FOUND);
    }
}