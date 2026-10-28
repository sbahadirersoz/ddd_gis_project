using gis.Domain.ResultPattern;
using MediatR;

namespace Gis.Application.Features.Line.Queries.FindLineByID;

public record FindLineByIDQuery(Guid id):IRequest<Result< FindLineByIDResponse>>;

