using gis.Domain.ResultPattern;
using MediatR;

namespace gis.ApplicationLayer.Features.Line.Queries.FindLineByID;

public record FindLineByIDQuery(Guid id):IRequest<Result< FindLineByIDResponse>>;

