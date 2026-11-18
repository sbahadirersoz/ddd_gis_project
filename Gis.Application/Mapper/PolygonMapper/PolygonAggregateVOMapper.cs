using Gis.Application.Dtos;
using gis.Domain.Entities.Coord;
using gis.Domain.Entities.Coord.Polygon;
using gis.Domain.Entities.Information.Polygon;
using gis.Domain.Entities.WKT;
using gis.Domain.ResultPattern;

namespace Gis.Application.Mapper.PolygonMapper;

public static class PolygonAggregateVOMapper
{
    public static Result<PolygonName> CreatePolyNameFromString(string polygonName)
    => PolygonName.CreateFromString(polygonName);
    public static Result<PolygonDescription> CreatePolyDescFromString(string polygonDesc)
    => PolygonDescription.CreateFromString(polygonDesc);

    public static Result<PolygonShell> CoordinateDtoListToPolygonShell(List<CoordinateDto> coords, WellKnownText wkt)
    {
        var list = DtoToValueObjects(coords);
        if (list.IsFailure) return Result<PolygonShell>.Failure(list.Error);
        return PolygonShell.Create(list.Value,wkt);
    }

    private  static Result<CoordinateValueObject> primitivesToCoordinates(double latitude, double longitude)
    {
        var latResult = Latitude.Create(latitude);
        if (latResult.IsFailure)   return Result<CoordinateValueObject>.Failure(latResult.Error);
        var lonResult = Longitude.Create(longitude);
        if (lonResult.IsFailure)   return Result<CoordinateValueObject>.Failure(lonResult.Error);
        
        return CoordinateValueObject.ForPolygonCoordCreation(latResult.Value!, lonResult.Value!);
    }

    public static Result<List<CoordinateValueObject>> DtoToValueObjects(List<CoordinateDto> dto)
    {
        var results = dto.Select(x=>
            primitivesToCoordinates(x.Latitude,x.Longitude)
        ).ToList();
        var failed =  results.FirstOrDefault(r=>r.IsFailure);
        return (failed != null)
            ? Result<List<CoordinateValueObject>>.Failure(failed.Error)
            : Result<List<CoordinateValueObject>>.Success(results.Select(x => x.Value!).ToList());
    }



}