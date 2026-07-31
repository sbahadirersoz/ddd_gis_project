using FluentAssertions;
using gis.Domain.Contracts;
using gis.Domain.Entities.Coord;
using gis.Domain.Entities.Information;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern.Errors;
using gis.Domain.Services;
using NSubstitute;

namespace Tests.UnitTests.Domain.PoiTests;

public class PoiDomainServiceTest
{
    private readonly POIDomainService _poiService;
    private readonly IPointRepository _pointRepository;
    private readonly ITopologySuitePointContract _contract;

    public PoiDomainServiceTest(POIDomainService poiService, IPointRepository pointRepository,
        ITopologySuitePointContract contract)
    {
        _pointRepository = Substitute.For<IPointRepository>();
        _contract = Substitute.For<ITopologySuitePointContract>();
        _poiService = new POIDomainService(_pointRepository, _contract);
    }

    [Fact]
    public async void CreatePoi()
    {
        var coords =  Coordinates.FromLatLon(40.7128, -74.0060,null).Value;
        var name =  PointName.FromString("Mock Location").Value;
        var desc = PointDescription.FromString("Central Park").Value;

        // Koordinatın olmadığını (false) fakat ismin veritabanında olduğunu (true) simüle ediyoruz
        _pointRepository.IsCoordinatesExistsAsync(coords).Returns(false);
        _pointRepository.IsPointNameExistsAsync(name).Returns(true);

        // Act
        var result = await _poiService.CreatePOIAggregate(coords, desc, name);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(RepositoryErrors.VALUE_ALREADY_EXIST_IN_DB);
        
        // Bu senaryoda iki metodun da birer kez çalıştırılmış olması gerekir
        await _pointRepository.Received(1).IsCoordinatesExistsAsync(coords);
        await _pointRepository.Received(1).IsPointNameExistsAsync(name);
    }
}