using System.Text.Json;
using gis.ApplicationLayer.Common;
using gis.ApplicationLayer.Dtos;
using gis.ApplicationLayer.Features.Poi.Queries.FindPoiById;
using gis.Domain.Repositories;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Tests.HelperMethods.Poi;
using Xunit.Abstractions;

namespace Tests.UnitTests.Application.Features.Poi.Queries.FindPoiById;

public class FindPoiByIdQueryHandlerTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<FindPoiByIdQueryHandler> _logger;
    private readonly FindPoiByIdQueryHandler _handler;
    private readonly IPointRepository _pointRepository;

    public FindPoiByIdQueryHandlerTests(ITestOutputHelper testOutputHelper)
    {
        this._testOutputHelper = testOutputHelper;
        _unitOfWork  = Substitute.For<IUnitOfWork>();
        _logger = Substitute.For<ILogger<FindPoiByIdQueryHandler>>();
        _pointRepository =  Substitute.For<IPointRepository>();
        _handler = new FindPoiByIdQueryHandler( _unitOfWork,_logger,_pointRepository);
    }
    [Fact]
    public async Task FindPoiByIdQueryHandler_SuccessCase()
    {
        
        var mockPrevPoiCreation = await PoiHelperTestMethods.MockPrevPoiCreation("PrevName","Description",new CoordinateDto(42,35));
        var request = new FindPoiByIdQuery(mockPrevPoiCreation.Id);
        _pointRepository.FindPointByIdAsync(request.id).Returns(mockPrevPoiCreation);
        var result = await _handler.Handle(request);
        if (result.IsFailure)
        {
            _testOutputHelper.WriteLine(result.Error.Code);
            _testOutputHelper.WriteLine(result.Error.Desc);
            return;
        }
        string json = JsonSerializer.Serialize(result.Value);
        _testOutputHelper.WriteLine(json);
            

        
    }
}