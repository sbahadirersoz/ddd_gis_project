using System.Runtime.InteropServices.JavaScript;

using FluentAssertions;

using Gis.Application.Dtos;
using Gis.Application.Features.Line.Queries.FindLineByID;

using gis.Domain.Entities.IDs;
using gis.Domain.Entities.Information;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern.Errors;

using Microsoft.Extensions.Logging;

using NSubstitute;
using NSubstitute.ReturnsExtensions;

using Tests.HelperMethods.Line;

namespace Tests.UnitTests.Application.Features.Line.Queries;

public class FindLineByIdHandlerTests
{

    private readonly ILineRepository _repository;
    private readonly ILogger<FindLineByIDHandler> _logger;
    private readonly FindLineByIDHandler _handler;

    public FindLineByIdHandlerTests()
    {
        _repository = Substitute.For<ILineRepository>();
        _logger = Substitute.For<ILogger<FindLineByIDHandler>>();
        _handler = new FindLineByIDHandler(_logger,_repository);
    }


    [Fact]
    public async Task FindLineById_Existing_ReturnsLine()
    {
        var entity = await LineHelperMethods.CreateMockLine(new CoordinateDto(43, 25), new CoordinateDto(46, 31), "lineName",
            "lineDesc");
        _repository.FindByIdAsync(Arg.Any<LineID>(),Arg.Any<bool>(),Arg.Any<CancellationToken>()).Returns(entity.Value);

        var mockq = new FindLineByIDQuery(Guid.NewGuid());
        var handle = await _handler.Handle(mockq);
        handle.Value.name.Should().Be("lineName");
        handle.Value.status.Should().Be(LineStatus.ACTIVE);
        handle.Value.desc.Should().Be("lineDesc");
        handle.Value.id.Should().Be(entity.Value.Id.Value);
    }
    [Fact]
    public async Task FindLineById_Missing_ReturnsENF()
    {
        _repository.FindByIdAsync(Arg.Any<LineID>(),Arg.Any<bool>(),Arg.Any<CancellationToken>()).ReturnsNull();

        var mockq = new FindLineByIDQuery(Guid.NewGuid());
        var handle = await _handler.Handle(mockq);
        handle.Error.Should().Be(RepositoryErrors.ENTITY_NOT_FOUND);
    }
}