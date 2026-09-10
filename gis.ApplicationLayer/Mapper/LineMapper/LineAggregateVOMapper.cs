using gis.Domain.Aggregates;
using gis.Domain.Entities.Coord;
using gis.Domain.Entities.Information.Line;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;

namespace gis.ApplicationLayer.Mapper.LineMapper;

public static class LineAggregateVOMapper
{
    public static Result<LineName> PrimitiveToLineName(string lineName) => LineName.FromString(lineName);
    public static Result<LineDescription> PrimitiveToLineDesc(string lineDesc)=> LineDescription.FromString(lineDesc);
    public static Result<List<CoordinateValueObject>> PrimitiveListToCoordinatesList
        (List<(double latitude, double longitude)> coordinates)
    {
        var results = coordinates.Select
                (x => PrimitivesToCordinate(x.latitude, x.longitude).Value)
            .Where(x => x != null).ToList();
        
        return (results.Count is 0)
            ? Result<List<CoordinateValueObject>>.Failure(DomainErrors.LineErrors.Coordinates
                .BAD_CREDENTIALS_FOR_CORDINATES)
            : Result<List<CoordinateValueObject>>.Success(results);

    }

    public static Result<CoordinateValueObject> PrimitivesToCordinate(double latitude, double longitude)
    {
        var lat = Latitude.Create(latitude);
        var lon = Longitude.Create(longitude);
        if (lat.IsFailure) return Result<CoordinateValueObject>.Failure(lat.Error);
        if (lon.IsFailure) return Result<CoordinateValueObject>.Failure(lon.Error);
        return CoordinateValueObject.ForLineCoordCreation(lat.Value, lon.Value);
    }
    
}