
using FluentAssertions;

using Gis.Application.Common;
using Gis.Application.Dtos;
using Gis.Application.Features.Polygon.Commands;

using gis.Domain.Aggregates;
using gis.Domain.BusinessRules.PolygonRules;
using gis.Domain.Entities.Information.Polygon;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern.Errors;
using gis.Domain.Services;

using Gis.Infrastructure.Topology_Services;

using NSubstitute;

namespace Tests.UnitTests.Application.Features.Polygon;

public class CreatePolygonCommandHandlerTest
{
    private readonly IPolygonRepository _repo = Substitute.For<IPolygonRepository>();
    private readonly IUnitOfWork _uow = Substitute.For<IUnitOfWork>();
    private readonly CreatePolygonCommandHandler _handler;

    public CreatePolygonCommandHandlerTest()
    {
        var wkt = new TopologySuiteWktContractImpl();
        var rules = new PolygonBusinessRulesFactory(new TopologySuitePolygonValidationContractImpl());
        var service = new PolygonDomainServiceContract(_repo, wkt, rules);
        _handler = new CreatePolygonCommandHandler(_repo, _uow, service, wkt);
        _repo.IsPolygonNameExistsAsync(Arg.Any<PolygonName>(), Arg.Any<CancellationToken>()).Returns(false);
    }

    private static CreatePolygonCommand Command(params (double lon, double lat)[] points) =>
        new(points.Select(p => new CoordinateDto(p.lon, p.lat)).ToList(), "park", "green area");

    private static readonly (double, double)[] ClosedSquare =
        [(29.00, 41.00), (29.10, 41.00), (29.10, 41.10), (29.00, 41.10), (29.00, 41.00)];

    private async Task NothingSaved()
    {
        await _repo.DidNotReceive().AddAsync(Arg.Any<PolygonAggregate>(), Arg.Any<CancellationToken>());
        await _uow.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ValidPolygon_IsSaved_AndReturnsPolygonWkt()
    {
        var result = await _handler.Handle(Command(ClosedSquare), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value!.name.Should().Be("park");
        result.Value.WKT.Should().StartWith("POLYGON");
        await _repo.Received(1).AddAsync(Arg.Is<PolygonAggregate>(a => a.Name.Value == "park"), Arg.Any<CancellationToken>());
        await _uow.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task NotClosedRing_ReturnsPolygonNotClosed()
    {
        var result = await _handler.Handle(
            Command((29.00, 41.00), (29.10, 41.00), (29.10, 41.10), (29.00, 41.10)), CancellationToken.None);

        result.Error.Should().Be(BusinessRuleErrors.PolygonRules.POLYGON_NOT_CLOSED);
        await NothingSaved();
    }

    [Fact]
    public async Task ClosedButTooFewPoints_ReturnsInsufficientPoints()
    {
        var result = await _handler.Handle(
            Command((29.00, 41.00), (29.10, 41.00), (29.00, 41.00)), CancellationToken.None);

        result.Error.Should().Be(BusinessRuleErrors.PolygonRules.INSUFFICENT_POINTS);
        await NothingSaved();
    }

    [Fact]
    public async Task LatitudeOutOfRange_ReturnsLatitudeError()
    {
        var result = await _handler.Handle(
            Command((29.00, 41.00), (29.10, 95.00), (29.10, 41.10), (29.00, 41.10), (29.00, 41.00)), CancellationToken.None);
        result.Error.Should().Be(DomainErrors.POIErrors.Coordinate.LATITUDE_COORDINATE_IS_OVER_MAX_VALUE_ERROR);
        await NothingSaved();
    }

    [Fact]
    public void PolygonRuleErrors_HaveDistinctCodes()
    {
        // otherwise a test expecting one would also pass on the other
        BusinessRuleErrors.PolygonRules.POLYGON_NOT_CLOSED.Code
            .Should().NotBe(BusinessRuleErrors.PolygonRules.INSUFFICENT_POINTS.Code);
    }
    [Fact]
    public async Task PolygonName_AlreadyExists_ReturnsAlreadyExist()
    {
        _repo.IsPolygonNameExistsAsync(Arg.Any<PolygonName>(), Arg.Any<CancellationToken>()).Returns(true);
        var result = await _handler.Handle(
            Command(ClosedSquare));
        result.Error.Should().Be(RepositoryErrors.VALUE_ALREADY_EXIST_IN_DB);
        await NothingSaved();
    }   [Fact]
    public async Task ThreePoints_NotCloses()
    {
        var result = await _handler.Handle(
            Command((22,42),(34,42),(35,47)));
        result.Error.Should().Be(BusinessRuleErrors.PolygonRules.INSUFFICENT_POINTS);
        await NothingSaved();
    }

    [Fact]
    public async Task SelfIntersectingShell_ReturnsSelfIntersecting()
    {
        // bowtie: the edges (29 41)-(29.1 41.1) and (29.1 41)-(29 41.1) cross
        var result = await _handler.Handle(
            Command((29.00, 41.00), (29.10, 41.10), (29.10, 41.00), (29.00, 41.10), (29.00, 41.00)), CancellationToken.None);

        result.Error.Should().Be(BusinessRuleErrors.PolygonRules.SELF_INTERSECTING);
        await NothingSaved();
    }
}
