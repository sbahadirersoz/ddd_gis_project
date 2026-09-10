using gis.ApplicationLayer.Common;
using gis.Domain.BusinessRules.PolygonRules.Abstract;
using gis.Domain.Contracts;
using gis.Domain.ResultPattern;
using MediatR;

namespace gis.ApplicationLayer.Features.Polygon.Commands.AddHoleToPolygon;

public class AddHoleToPolygonCommandHandler:IRequestHandler<AddHoleToPolygonCommand,Result<AddHoleToPolygonResponse>>
{

    private readonly IPolygonDomainServiceContract _service;
    private readonly IUnitOfWork _uow;

    public AddHoleToPolygonCommandHandler(IPolygonDomainServiceContract service, IUnitOfWork uow)
    {
        _service = service;
        _uow = uow;
    }

    public Task<Result<AddHoleToPolygonResponse>> Handle(AddHoleToPolygonCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}