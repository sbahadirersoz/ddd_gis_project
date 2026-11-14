using FluentAssertions;

using Gis.Application.Dtos;
using Gis.Application.Features.Line.Queries.Distance.LonLat.FindClosestByLonLat;

using gis.Domain.Entities.Coord;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern.Errors;

using Microsoft.Extensions.Logging;

using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace Tests.UnitTests.Application.Features.Line.Queries.Distance.Lonlat.ClosestByLonLat;

public class ClosestByLonLatHandlerTest
{
    private  readonly  ILogger<FindClosestLineByLonLatHandler> _logger;
    private  readonly  ILineRepository _repository;
    private  readonly  FindClosestLineByLonLatHandler _handler;

    public ClosestByLonLatHandlerTest()
    {
        _logger = Substitute.For<ILogger<FindClosestLineByLonLatHandler>>();
        _repository = Substitute.For<ILineRepository>();
        _handler = new FindClosestLineByLonLatHandler(_logger,_repository);
    }

    [Fact]
    public async Task ClosestByLonLat_NoLines_ReturnsNoNeighbor()
    {
        _repository.FindClosestByLonLatAsync(Arg.Any<Longitude>(), Arg.Any<Latitude>(), Arg.Any<bool>(),
            Arg.Any<CancellationToken>()).ReturnsNull();
        var mockq = new FindClosestLineByLonLatQuery(new CoordinateDto(45,54));

        var handle = await _handler.Handle(mockq);
        handle.Error.Should().Be(RepositoryErrors.NO_NEIGHBOR_FOUND);
    }

}