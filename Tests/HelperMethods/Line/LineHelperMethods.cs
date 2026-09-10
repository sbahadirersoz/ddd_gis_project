using gis.ApplicationLayer.Features.Line.Commands.Create;
using gis.ApplicationLayer.Mapper.LineMapper;
using gis.Domain.Aggregates;
using gis.Domain.Contracts;
using gis.Domain.Entities.Coord;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern;
using gis.Domain.Services;
using NSubstitute;

namespace Tests.HelperMethods.Line;

public class LineHelperMethods
{
    private static readonly ILineRepository _repository;
    private static readonly LineDomainServiceContract _service;
    private static readonly ITopologySuiteWKTContract _contract;

     static LineHelperMethods()
    {
        _repository = Substitute.For<ILineRepository>();
        _contract = Substitute.For<ITopologySuiteWKTContract>();
        _service = new LineDomainServiceContract(_repository, _contract);
    }

    public static async Task<Result<LineAggregate>> CreateMockLine(CreateLineCommand cmd)
    {
        _contract.CreateWktStringFromCoordList(Arg.Any<List<CoordinateValueObject>>()).Returns(Result<string>.Success("POLYGON (35,42)"));
        
        var list = cmd.coordinates.Select(c => (x: c.Longitude, y: c.Latitude)).ToList();
        var primitiveToCoordinates = LineAggregateVOMapper.PrimitiveListToCoordinatesList(list).Value;
        var primitiveToLineName = LineAggregateVOMapper.PrimitiveToLineName(cmd.lineName).Value;
        var primitiveToLineDesc = LineAggregateVOMapper.PrimitiveToLineDesc(cmd.lineDescription).Value;
        var result = await _service.CreateLineAggregate(primitiveToCoordinates,primitiveToLineName,primitiveToLineDesc);
        return result.IsFailure ? Result<LineAggregate>.Failure(result.Error) : Result<LineAggregate>.Success(result.Value);
    }
}