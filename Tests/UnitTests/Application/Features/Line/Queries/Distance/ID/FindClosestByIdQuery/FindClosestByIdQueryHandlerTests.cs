using FluentAssertions;

using Gis.Application.Features.Line.Queries.Distance.ID.FindClosestByIdQuery;
using Gis.Application.Features.Poi.Queries.FindNearestPoi;

using gis.Domain.Entities.IDs;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;

using Microsoft.Extensions.Logging;

using NSubstitute;

namespace Tests.UnitTests.Application.Features.Line.Queries.Distance.ID.FindClosestByIdQuery;

public class FindClosestByIdQueryHandlerTests
{
    
    private readonly  ILogger<FindClosestLineByIdHandler> _logger;
    private readonly  ILineRepository _repo;
    private readonly FindClosestLineByIdHandler _handler;

    public FindClosestByIdQueryHandlerTests()
    {
        _logger = Substitute.For<ILogger<FindClosestLineByIdHandler>>();
        _repo = Substitute.For<ILineRepository>();
        _handler = new FindClosestLineByIdHandler(_logger, _repo);
    }


    [Fact]
    public async Task FIND_CLOSEST_BY_ID_QUERY_RETURNS_ENTITY_NOT_FOUND()
    {
        var mockq = new FindClosestLineByIdQuery(Guid.NewGuid());
        var handle =await _handler.Handle(mockq);
        handle.Error.Should().Be(RepositoryErrors.ENTITY_NOT_FOUND);

    }

}