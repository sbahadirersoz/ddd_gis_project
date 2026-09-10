using gis.ApplicationLayer.Dtos;
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
    private static readonly ITopologySuiteWKTContract _contract;
    private static readonly POIDomainService _service;
    private static readonly IPointRepository _repository;

    static PoiHelperTestMethods()
    {
    _contract = Substitute.For<ITopologySuiteWKTContract>();
    _repository = Substitute.For<IPointRepository>();
    _service = new POIDomainService(_repository,_contract);
    }
    public static async Task<POIAggregate> MockPrevPoiCreation(string mockName,string mockDesc,CoordinateDto dto)
    {
        var mockCommand = new CreatePoiCommand
        (
            mockName,
            mockDesc,
            dto
        );
        
        var descFromString = PointDescription.FromString(mockCommand.PointDesc).Value;
        var nameFromString = PointName.FromString(mockCommand.PoiName).Value;
        var lon = Longitude.Create(dto.Longitude);
        var lat = Latitude.Create(dto.Latitude);
        
        _contract.CreateWktStringFromLonLat(lat.Value ,lon.Value).Returns(Result<string>.Success($"POINT ({lat.Value} {lon.Value})"));
        var poiAggregate = await _service.CreatePoiAggregate(lat.Value, lon.Value,descFromString, nameFromString);
        return poiAggregate.Value;
    }    
    public static async Task<List<POIAggregate>> mockList()
    {
        var unit1 =  await PoiHelperTestMethods.MockPrevPoiCreation("unit1","unit1desc",new CoordinateDto(41,42) );
        var unit2 = await PoiHelperTestMethods.MockPrevPoiCreation("unit2","unit2desc",new CoordinateDto(41,43));
        var unit3 = await PoiHelperTestMethods.MockPrevPoiCreation("unit3","unit3desc",new CoordinateDto(41,44));
        List<POIAggregate> l = new List<POIAggregate>();
        l.Add(unit1);
        l.Add(unit2);
        l.Add(unit3);
        return l;
    }

}