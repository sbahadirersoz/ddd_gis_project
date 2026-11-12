using FluentAssertions;

using Gis.Application.Common;
using Gis.Application.Dtos;
using Gis.Application.Features.Line.Commands.Update;

using gis.Domain.Contracts;
using gis.Domain.Entities.IDs;
using gis.Domain.Entities.Information.Line;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern.Errors;
using gis.Domain.Services;

using Gis.Infrastructure.Topology_Services;

using Microsoft.Extensions.Logging;

using NSubstitute;
using NSubstitute.ReturnsExtensions;

using Tests.HelperMethods.Line;

namespace Tests.UnitTests.Application.Features.Line.Command;

public class UpdateLineCommandHandlerTests
{
    private readonly ILineRepository _repository;
    private readonly ILogger<UpdateLineCommandHandler> _logger;
    private readonly ITopologySuiteWKTContract _contract;
    private readonly IUnitOfWork _uow;
    private readonly LineDomainServiceContract _service;
    private readonly UpdateLineCommandHandler _handler;

    public UpdateLineCommandHandlerTests()
    {
        _repository = Substitute.For<ILineRepository>();
        _logger = Substitute.For<ILogger<UpdateLineCommandHandler>>();
        _contract = new TopologySuiteWktContractImpl();
        _uow = Substitute.For<IUnitOfWork>();
        _service = new LineDomainServiceContract(_repository, _contract);
        _handler = new UpdateLineCommandHandler(_service, _logger, _repository, _uow);
    }

    [Fact]
    public async Task UpdateLineCommand_OnlyDescription_Succeeds()
    {
        var entity = await LineHelperMethods.CreateMockLine(new CoordinateDto(44, 43), new CoordinateDto(46, 26),
            "mockLineName", "lineDesc");
        _repository.FindByIdAsync(Arg.Any<LineID>(), Arg.Any<bool>(), Arg.Any<CancellationToken>())
            .Returns(entity.Value);

        var mockCmd = new UpdateLineCommand(Guid.NewGuid(), LineDescription: "TestDesc");
        var handle = await _handler.Handle(mockCmd);
        handle.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateLineCommand_OnlyCoordinates_Succeeds()
    {
        var entity = await LineHelperMethods.CreateMockLine(new CoordinateDto(44, 43), new CoordinateDto(46, 26),
            linename: "mockLineName", linedesc: "lineDesc");
        _repository.IsLineNameExistsAsync(Arg.Any<LineName>()).Returns(false);
        _repository.FindByIdAsync(Arg.Any<LineID>(), Arg.Any<bool>(), Arg.Any<CancellationToken>())
            .Returns(entity.Value);

        var list = new List<CoordinateDto>();
        list.Add(new CoordinateDto(54, 32));
        list.Add(new CoordinateDto(57, 30));
        var mockCmd = new UpdateLineCommand(Guid.NewGuid(), Coordinates: list);
        var handle = await _handler.Handle(mockCmd);
        handle.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateLineCommand_LineNotFound_ReturnsEntityNotFound()
    {
        _repository.FindByIdAsync(Arg.Any<LineID>(),Arg.Any<bool>(),Arg.Any<CancellationToken>()).ReturnsNull();

        var mockCmd = new UpdateLineCommand(Guid.NewGuid());
        var handle = await _handler.Handle(mockCmd);
        handle.IsFailure.Should().BeTrue();
        handle.Error.Should().Be(RepositoryErrors.ENTITY_NOT_FOUND);
    }
}