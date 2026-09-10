using System.Text.Json;
using gis.ApplicationLayer.Common;
using gis.ApplicationLayer.Dtos;
using gis.ApplicationLayer.Features.Polygon.Commands;
using gis.Domain.BusinessRules.PolygonRules.Abstract;
using gis.Domain.Contracts;
using gis.Domain.Entities.Coord;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern;
using gis.Domain.Services;
using NSubstitute;
using Xunit.Abstractions;

namespace Tests.UnitTests.Application.Features.Polygon;

public class CreatePolygonCommandHandlerTest
{
    private readonly ITestOutputHelper _output;
    private readonly IPolygonRepository _repo;
    private readonly IPolygonDomainServiceContract _service;
    private readonly CreatePolygonCommandHandler _handler;
    private readonly ITopologySuiteWKTContract _wkt;
    private readonly IUnitOfWork _uow;
    private readonly  IPolygonBusinessRulesFactory _validationFactory;

    public CreatePolygonCommandHandlerTest(ITestOutputHelper  helper)
    {
        _output = helper;
        _uow = Substitute.For<IUnitOfWork>();
        _repo = Substitute.For<IPolygonRepository>();
        _wkt = Substitute.For<ITopologySuiteWKTContract>();
        _validationFactory = Substitute.For<IPolygonBusinessRulesFactory>();
        _service = new PolygonDomainServiceContract(_repo, _wkt,_validationFactory);
        _handler = new CreatePolygonCommandHandler(_repo, _uow,_service, _wkt);
    }

    [Fact]
    public async Task CreatePolygon()
    {
        var mockComm = new CreatePolygonCommand(new List<CoordinateDto>
            {
                new CoordinateDto(45, 32),
                new CoordinateDto(47, 35),
                new CoordinateDto(41, 40)
            },
            "TestPolyName",
            "FreeDesc"
        );
        
        
        _wkt.CreateWktStringFromCoordList(Arg.Any<List<CoordinateValueObject>>()).Returns(Result<string>.Success("POLYGON (35,42)"));
        var handle = await _handler.Handle(mockComm);
        if (handle.IsFailure)
        {
            _output.WriteLine(handle.Error.Desc);
            _output.WriteLine(handle.Error.Code);
            return;
        }
        _output.WriteLine(JsonSerializer.Serialize(handle.Value));
    }
}