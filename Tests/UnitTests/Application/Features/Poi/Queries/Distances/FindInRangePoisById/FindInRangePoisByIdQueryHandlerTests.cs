using FluentAssertions;

using Gis.Application.Common;
using Gis.Application.Dtos;
using Gis.Application.Features.Poi.Queries.Distance.Id.FindInRangePoisById;
using Gis.Application.Features.Poi.Queries.FindPoisInGivenRange;

using gis.Domain.Aggregates;
using gis.Domain.Contracts;
using gis.Domain.Entities.Coord;
using gis.Domain.Entities.IDs;
using gis.Domain.Repositories;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;

using Microsoft.Extensions.Logging;

using NSubstitute;
using NSubstitute.ReturnsExtensions;

using Tests.HelperMethods.Poi;

using Xunit.Abstractions;

namespace Tests.UnitTests.Application.Features.Poi.Queries.FindInRangePoisById;

public class FindInRangePoisByIdQueryHandlerTests
{
    
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly ILogger<FindInRangePoisByIdQueryHandler> _logger;
    private readonly FindInRangePoisByIdQueryHandler _handler;
    private readonly IPointRepository _pointRepository;
    private readonly IUnitOfWork _unitOfWork;
    

    public FindInRangePoisByIdQueryHandlerTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _logger = Substitute.For<ILogger<FindInRangePoisByIdQueryHandler>>();
        _pointRepository = Substitute.For<IPointRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new FindInRangePoisByIdQueryHandler(_logger, _pointRepository,_unitOfWork);
    }


    
    [Fact]
    public async Task FIND_INRANGE_BY_ID_POI_NO_FOUND()
    {
        _pointRepository.FindPointByIdAsync(Arg.Any<PointID>()).ReturnsNull();
        var mockQuery = new FindInRangePoisByIdQuery(PointID.New().Value, 3500);
        var result = await _handler.Handle(mockQuery);

        result.IsFailure.Should().Be(true);
        result.Error.Should().Be(RepositoryErrors.ENTITY_NOT_FOUND);
    }
    [Fact]
    public async Task FIND_INRANGE_BY_ID_NEIGHBOR_DOESNT_EXISTS()
    {
        var mockPoi = await PoiHelperTestMethods.MockPoiCreation("mock","Mockdesc",new CoordinateDto(44,32));
        _pointRepository.FindPointByIdAsync(Arg.Any<PointID>()).Returns(mockPoi);
        _pointRepository.FindInRangePoisByIdAsync(Arg.Any<PointID>(),Arg.Any<double>()).Returns(new List<(POIAggregate agg, double distance)>() );
        
        
        var mockQuery = new FindInRangePoisByIdQuery(PointID.New().Value, 3500);
        var result = await _handler.Handle(mockQuery);
        var list = await PoiHelperTestMethods.mockListWithDouble();
        var res = list.ConvertAll(x=>FindInRangePoisByIdQueryResponse.FromAgg(x.Item1,x.Item2));
        result.Value.Should().BeEquivalentTo(new List<FindInRangePoisByIdQueryResponse>());

    }

    [Fact]
    public async Task FIND_INRANGE_BY_ID_SUCCESS()
    {
        var list = await PoiHelperTestMethods.mockListWithDouble();
        var mockPoiCreation = await PoiHelperTestMethods.MockPoiCreation("mock","Mockdesc",new CoordinateDto(44,32));
        _pointRepository.FindPointByIdAsync(Arg.Any<PointID>()).Returns(mockPoiCreation);
        _pointRepository.FindInRangePoisByIdAsync(Arg.Any<PointID>(),Arg.Any<double>()).Returns(list);
        var mockQuery = new FindInRangePoisByIdQuery(PointID.New().Value, 3500);
        var result = await _handler.Handle(mockQuery);
        var res = list.ConvertAll(x=>FindInRangePoisByIdQueryResponse.FromAgg(x.Item1,x.Item2));
        result.Value.Should().BeEquivalentTo(res ,o  =>  o.WithStrictOrdering());
    }


}