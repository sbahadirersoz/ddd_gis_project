using gis.Domain.ResultPattern;
using MediatR;

namespace gis.ApplicationLayer.Features.Line.Queries.FindLineByName;

public record FindLineByNameQuery(string name):IRequest<Result< FindLineByNameResponse>>;
