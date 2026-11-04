using gis.Domain.ResultPattern;
using MediatR;

namespace Gis.Application.Features.Line.Queries.FindLineByName;

public record FindLineByNameQuery(string name):IRequest<Result< FindLineByNameResponse>>;
