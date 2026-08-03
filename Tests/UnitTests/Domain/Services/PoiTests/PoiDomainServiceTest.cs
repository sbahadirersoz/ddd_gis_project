using FluentAssertions;
using gis.Domain.Contracts;
using gis.Domain.Entities.Coord;
using gis.Domain.Entities.Information;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern.Errors;
using gis.Domain.Services;
using NSubstitute;
using Xunit.Abstractions;

namespace Tests.UnitTests.Domain.Services.PoiTests;

public class PoiDomainServiceTest
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly POIDomainService _poiService;
    private readonly IPointRepository _pointRepository;
    private readonly ITopologySuitePointContract _contract;

    public PoiDomainServiceTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _pointRepository = Substitute.For<IPointRepository>();
        _contract = Substitute.For<ITopologySuitePointContract>();
        _poiService = new POIDomainService(_pointRepository, _contract);
    }

    [Fact]
    public async Task  CreatePoi_CoordExist_Case()
    {

        var lat = Latitude.Create(40.7128).Value;
        var lon = Longitude.Create(40.7128).Value;
        var desc = PointDescription.FromString("Central Park").Value;
        var name =  PointName.FromString("Mock Location").Value;

        // Koordinatın olmadığını (false) fakat ismin veritabanında olduğunu (true) simüle ediyoruz
        _pointRepository.IsLatLonCoordinatesExistsAsync(lat,lon).Returns(false);
        _pointRepository.IsPointNameExistsAsync(name).Returns(true);

        // Act
        var result = await _poiService.CreatePoiAggregate(lat,lon, desc, name);

        // Assert
        _testOutputHelper.WriteLine(result.Error.ToString());
        result.Error.Should().Be(RepositoryErrors.VALUE_ALREADY_EXIST_IN_DB);
        result.IsFailure.Should().BeTrue();

        await _pointRepository.Received(1).IsLatLonCoordinatesExistsAsync(lat,lon);
        await _pointRepository.Received(1).IsPointNameExistsAsync(name);
    } 
[Fact]
    public async Task  CreatePoi_Success_Case()
    {

        var lat = Latitude.Create(40.7128).Value;
        var lon = Longitude.Create(40.7128).Value;
        var desc = PointDescription.FromString("Central Park").Value;
        var name =  PointName.FromString("Mock Location").Value;

        _pointRepository.IsLatLonCoordinatesExistsAsync(lat,lon).Returns(false);
        _pointRepository.IsPointNameExistsAsync(name).Returns(false);
        _contract.CreateWktStringFromLatLon(lat, lon).Returns($"POINT ({lat.Value} {lon.Value})");

        // Act
        var result = await _poiService.CreatePoiAggregate(lat,lon, desc, name);
        if (result.IsFailure)
        {
            _testOutputHelper.WriteLine(result.Error.Code);
        }

        
        // Assert

        _testOutputHelper.WriteLine(result.Value.Coordinates.Latitude.ToString());
        _testOutputHelper.WriteLine(result.Value.Coordinates.Longitude.ToString());
        _testOutputHelper.WriteLine(result.Value.Coordinates.WKT.Value);
        result.IsSuccess.Should().BeTrue();
    

        await _pointRepository.Received(1).IsLatLonCoordinatesExistsAsync(lat,lon);
        await _pointRepository.Received(1).IsPointNameExistsAsync(name);
    }
    [Fact]
    public async Task CreatePoi_NameNull_Case()
    {

        var lat = Latitude.Create(40.7128).Value;
        var lon = Longitude.Create(40.7128).Value;
        var desc = PointDescription.FromString("Central Park").Value;
        var name =  PointName.FromString("");

        _pointRepository.IsLatLonCoordinatesExistsAsync(lat,lon).Returns(false);
        _pointRepository.IsPointNameExistsAsync(name.Value).Returns(false);
        _contract.CreateWktStringFromLatLon(lat, lon).Returns($"POINT ({lat.Value} {lon.Value})");

        // Act
        var result = await _poiService.CreatePoiAggregate(lat,lon, desc, name.Value);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.POIErrors.WKT.INVALID_WKT_FORMAT);
        _testOutputHelper.WriteLine(result.Error.ToString());

        await _pointRepository.Received(1).IsPointNameExistsAsync(name.Value);
        await _pointRepository.Received(1).IsLatLonCoordinatesExistsAsync(lat,lon);
    }
[Fact]
    public async Task UpdatePoiAggregate()
    {
        var lat = Latitude.Create(40.7128).Value;
        var lon = Longitude.Create(40.7128).Value;
        var desc = PointDescription.FromString("Central Park").Value;
        var name =  PointName.FromString("MockLocation").Value;
        _pointRepository.IsLatLonCoordinatesExistsAsync(lat,lon).Returns(false);
        _pointRepository.IsPointNameExistsAsync(name).Returns(false);

        var result = await _poiService.CreatePoiAggregate(lat,lon, desc, name);
        
        _testOutputHelper.WriteLine(result.Value.Coordinates.Longitude.ToString());
        _testOutputHelper.WriteLine(result.Value.Coordinates.Latitude.ToString());
        _testOutputHelper.WriteLine(result.Value.Coordinates.WKT.ToString());
        _testOutputHelper.WriteLine(result.Value.PointDesc.ToString());
        _testOutputHelper.WriteLine(result.Value.PointName.ToString());
        _testOutputHelper.WriteLine(result.Value.Status.ToString());
        
    }
    }
