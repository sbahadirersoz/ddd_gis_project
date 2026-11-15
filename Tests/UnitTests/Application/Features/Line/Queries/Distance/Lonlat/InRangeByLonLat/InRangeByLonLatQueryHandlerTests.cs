using FluentAssertions;

using Gis.Application.Dtos;
using Gis.Application.Features.Line.Queries.Distance.LonLat.FindInRangeByLonLat;

using gis.Domain.Aggregates;
using gis.Domain.Entities.Coord;
using gis.Domain.Repositories;

using Microsoft.Extensions.Logging;

using NSubstitute;

namespace Tests.UnitTests.Application.Features.Line.Queries.Distance.Lonlat.InRangeByLonLat;

public class InRangeByLonLatQueryHandlerTests
{
    private readonly ILogger<FindInRangeLineByLonLatHandler> _logger;
    private readonly ILineRepository _repo;
    private readonly FindInRangeLineByLonLatHandler _handler;


    public InRangeByLonLatQueryHandlerTests()
    {
        _logger = Substitute.For<ILogger<FindInRangeLineByLonLatHandler>>();
        _repo = Substitute.For<ILineRepository>();
        _handler = new FindInRangeLineByLonLatHandler(_logger, _repo);        
    }


    [Fact]
    public async Task InRangeByLonLat_Empty_ReturnsEmptyList ()
    {
        var mockq = new FindInRangeLineByLonLatQuery(new CoordinateDto(42, 35), 555);
        _repo.FindInRangeLinesByLonLatAsync(Arg.Any<Longitude>(), Arg.Any<Latitude>(), Arg.Any<double>(),
                Arg.Any<bool>(), Arg.Any<CancellationToken>())
            .Returns(new List<(LineAggregate agg, double distance)>());
        var handle = await _handler.Handle(mockq);
        handle.IsSuccess.Should().BeTrue();
    }
}