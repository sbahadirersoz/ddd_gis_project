using gis.ApplicationLayer.Common;
using gis.ApplicationLayer.Features.Poi.Commands.Create;
using gis.ApplicationLayer.Features.Poi.Commands.Delete;
using gis.Domain.Contracts;
using gis.Domain.Repositories;
using gis.Domain.Services;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Tests.HelperMethods.Poi;
using Xunit.Abstractions;

namespace Tests.UnitTests.Application.Features.Poi.Commands.Delete;

public class DeletePoiCommandHandlerTests
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly ITopologySuiteWKTContract _contract;
    private readonly POIDomainService _service;
    private readonly IPointRepository _repository;
    private readonly DeletePoiCommandHandler _handler;
    private readonly ILogger<CreatePoiCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    
    public DeletePoiCommandHandlerTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _logger = Substitute.For<ILogger<CreatePoiCommandHandler>>();
        _unitOfWork  = Substitute.For<IUnitOfWork>();
        _repository = Substitute.For<IPointRepository>();
        _contract = Substitute.For<ITopologySuiteWKTContract>();
        _service = new POIDomainService(_repository,_contract);
        _handler = new DeletePoiCommandHandler(_unitOfWork, _repository, _logger, _service);
    }
    [Fact]
    public async void DeletePoiCommandHandler_SuccessCase()
    {
        
        var mockPrevPoiCreation = await PoiHelperTestMethods.MockPrevPoiCreation("PrevName","Description",42,42);
        //     "TestName",
        //     "Description",
        //     42,
        //     42
        var request = new DeletePoiCommand(mockPrevPoiCreation.Id);
        _repository.FindPointByIdAsync(request.id).Returns(mockPrevPoiCreation);
        var result = await _handler.Handle(request);
        if (result.IsFailure)
        {
            _testOutputHelper.WriteLine(result.Error.Code);
            _testOutputHelper.WriteLine(result.Error.Desc);
            return;
        }

        _testOutputHelper.WriteLine(result.Value.WKT);
        _testOutputHelper.WriteLine(result.Value.Status);
    }
}
