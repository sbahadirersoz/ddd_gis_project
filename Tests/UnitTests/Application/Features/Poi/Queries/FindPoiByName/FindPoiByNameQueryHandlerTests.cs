using System.Linq.Expressions;
using gis.ApplicationLayer.Common;
using gis.ApplicationLayer.Features.Poi.Queries.FindPoiByName;
using gis.Domain.Aggregates;
using gis.Domain.Repositories;
using gis.InfrastructureLayer.Repositories;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Tests.HelperMethods.Poi;
using Xunit.Abstractions;

namespace Tests.UnitTests.Application.Features.Poi.Queries.FindPoiByName;

public class FindPoiByNameQueryHandlerTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<FindPoiByNameQueryHandler> _logger;
    private readonly FindPoiByNameQueryHandler _handler;
    private readonly IPointRepository _pointRepository;

    public FindPoiByNameQueryHandlerTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _logger = Substitute.For<ILogger<FindPoiByNameQueryHandler>>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _pointRepository =  Substitute.For<IPointRepository>();
        _handler = new FindPoiByNameQueryHandler(_logger, _unitOfWork,_pointRepository);
    }

    [Fact]
    public async Task FindPoiByIdQueryHandler_SuccessCase()
    {

        var mockPrevPoiCreation = await PoiHelperTestMethods.MockPrevPoiCreation("PrevName", "Description", 42, 42);
        var request = new FindPoiByNameQuery(mockPrevPoiCreation.PointName);
        _pointRepository.FindByEntityExpressionAsync(Arg.Any<Expression<Func<POIAggregate,bool>>>()).Returns(mockPrevPoiCreation);
        var result = await _handler.Handle(request);
        if (result.IsFailure)
        {
            _testOutputHelper.WriteLine(result.Error.Code);
            _testOutputHelper.WriteLine(result.Error.Desc);
            return;
        }

        _testOutputHelper.WriteLine(result.Value.Status.ToString());
        _testOutputHelper.WriteLine(result.Value.Desc.ToString());


    }
}