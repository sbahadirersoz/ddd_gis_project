using gis.ApplicationLayer.Features.Poi.Commands.Create;
using gis.Domain.Aggregates;
using gis.Domain.Contracts;
using gis.Domain.Entities.Coord;
using gis.Domain.Entities.Information;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern;
using gis.Domain.Services;
using NSubstitute;

namespace Tests.HelperMethods.Poi;

public static class PoiHelperTestMethods
{
    private static readonly ITopologySuitePointContract _contract;
    private static readonly POIDomainService _service;
    private static readonly IPointRepository _repository;

    static PoiHelperTestMethods()
    {
    _contract = Substitute.For<ITopologySuitePointContract>();
    _repository = Substitute.For<IPointRepository>();
    _service = new POIDomainService(_repository,_contract);
    }
    public static async Task<POIAggregate> MockPrevPoiCreation(string taskName,string taskDesc,double latitude,double longitude)
    {
        var mockCommand = new CreatePoiCommand
        (
            taskName,
            taskDesc,
            latitude,
            longitude
        );
        
        var lat = Latitude.Create(mockCommand.Latitude).Value;
        var lon = Longitude.Create(mockCommand.Longitude).Value;
        var descFromString = PointDescription.FromString(mockCommand.PointDesc).Value;
        var nameFromString = PointName.FromString(mockCommand.PoiName).Value;
        _contract.CreateWktStringFromLatLon(lat,lon).Returns(Result<string>.Success($"POINT ({lat.Value} {lon.Value})"));
        var poiAggregate = await _service.CreatePoiAggregate(lat, lon,descFromString, nameFromString);
        return poiAggregate.Value;
    }    
}