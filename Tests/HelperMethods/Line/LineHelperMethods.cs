using Gis.Application.Dtos;
using Gis.Application.Features.Line.Commands.Create;
using Gis.Application.Mapper.LineMapper;
using gis.Domain.Aggregates;
using gis.Domain.Contracts;
using gis.Domain.Entities.Coord;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern;
using gis.Domain.Services;

using Gis.Infrastructure.Topology_Services;

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
        _contract = new TopologySuiteWktContractImpl();
        _service = new LineDomainServiceContract(_repository, _contract);
    }

    public static async Task<Result<LineAggregate>> CreateMockLine(CoordinateDto first,CoordinateDto second,string linename,string  linedesc)
    {
        
        var cmd = new CreateLineCommand(
            coordinates: new List<CoordinateDto>
            {
                first,
                second
            },
            lineName: linename,
            lineDescription: linedesc
        );

        
        var list = cmd.coordinates.Select(c => (x: c.Longitude, y: c.Latitude)).ToList();
        var primitiveToCoordinates = LineAggregateVOMapper.PrimitiveListToCoordinatesList(list).Value;
        var primitiveToLineName = LineAggregateVOMapper.PrimitiveToLineName(cmd.lineName).Value;
        var primitiveToLineDesc = LineAggregateVOMapper.PrimitiveToLineDesc(cmd.lineDescription).Value;
        var result = await _service.CreateLineAggregate(primitiveToCoordinates,primitiveToLineName,primitiveToLineDesc);
        return result.IsFailure ? Result<LineAggregate>.Failure(result.Error) : Result<LineAggregate>.Success(result.Value);
    }
}