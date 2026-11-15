using FluentAssertions;

using Gis.Application.Dtos;
using Gis.Application.Features.Line.Queries.Distance.ID.FindMostClosesCountByIdQuery;

using gis.Domain.Aggregates;
using gis.Domain.Entities.IDs;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern.Errors;

using Microsoft.Extensions.Logging;

using NSubstitute;
using NSubstitute.ReturnsExtensions;

using Tests.HelperMethods.Line;

namespace Tests.UnitTests.Application.Features.Line.Queries.Distance.ID.FindMostClosesCountByIdQuery;

public class FindMostClosesCountByIdHandlerTests
{
    private readonly ILogger<FindMostClosesCountByIdHandler> _logger;
    private readonly ILineRepository _repo;
    private readonly FindMostClosesCountByIdHandler _handler;


    public FindMostClosesCountByIdHandlerTests()
    {
        _logger = Substitute.For<ILogger<FindMostClosesCountByIdHandler>>();
        _repo = Substitute.For<ILineRepository>();
        _handler = new FindMostClosesCountByIdHandler(_logger, _repo);        
    }
    [Fact]
    public async Task FIND_MOST_CLOSES_LINES_BY_ID_RETURN_EMPTY_LIST()
    {
        var entity = await LineHelperMethods.CreateMockLine(new CoordinateDto(37,31),new CoordinateDto(44,22),"mockName","mockdesc");
        _repo.FindByIdAsync(Arg.Any<LineID>()).Returns(entity.Value);
        _repo.FindMostClosesCountByIdAsync(Arg.Any<LineID>(),Arg.Any<int>(),Arg.Any<bool>(),Arg.Any<CancellationToken>()).Returns(new List<(LineAggregate entity, double distance)>());
        var mockq = new Gis.Application.Features.Line.Queries.Distance.ID.FindMostClosesCountByIdQuery.FindMostClosesCountByIdQuery(Guid.NewGuid(),5);
        var handle = await _handler.Handle(mockq);
        handle.IsFailure.Should().BeFalse();
        handle.Value.Should().BeEmpty();
        _repo.Received(1).FindByIdAsync(Arg.Any<LineID>());
        _repo.Received(1).FindMostClosesCountByIdAsync(Arg.Any<LineID>(),Arg.Is(5),Arg.Is(true),Arg.Any<CancellationToken>());
    } 
    [Fact]
    public async Task FIND_MOST_CLOSES_LINES_BY_ID_RETURN_ENTITY_NOT_FOUND()
    {
        _repo.FindByIdAsync(Arg.Any<LineID>()).ReturnsNull();
        var mockq = new Gis.Application.Features.Line.Queries.Distance.ID.FindMostClosesCountByIdQuery.FindMostClosesCountByIdQuery(Guid.NewGuid(),5);
        var handle = await _handler.Handle(mockq);
        handle.IsFailure.Should().BeTrue();
        handle.Error.Should().Be(RepositoryErrors.ENTITY_NOT_FOUND);
        
        _repo.Received(1).FindByIdAsync(Arg.Any<LineID>());
        _repo.DidNotReceive().FindMostClosesCountByIdAsync(Arg.Any<LineID>(),Arg.Any<int>(),Arg.Any<bool>(),Arg.Any<CancellationToken>());
        
    }
    
}