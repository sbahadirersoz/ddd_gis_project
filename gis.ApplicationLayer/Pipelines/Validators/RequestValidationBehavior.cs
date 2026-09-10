using System.Reflection;
using FluentValidation;
using gis.Domain.ResultPattern;
using MediatR;

namespace gis.ApplicationLayer.Pipelines.Validators;

public class RequestValidationBehavior<TRequest,TResponse>:IPipelineBehavior<TRequest,TResponse>
where TResponse : Result
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public RequestValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }


    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);
            var results =  await Task.WhenAll(_validators.Select(v=> v.ValidateAsync(context, cancellationToken)));
            var failures = results.SelectMany(x=> x.Errors).Where(f => f != null).ToList();
            
            if (failures.Count != 0)
            { 
                var allErrorMsgs = string.Join("", failures.Select(f => f.ErrorMessage));
                var error = new Error(failures[0].ErrorCode, allErrorMsgs);
                var  getMethod = (typeof(TResponse)).GetMethod("Failure",BindingFlags.FlattenHierarchy|BindingFlags.Public | BindingFlags.Static);
                if (getMethod != null)
                {
                    var result = getMethod.Invoke(null, new object[] { error });
                    return (TResponse)result!;
                }
            }
        }
        return await next(cancellationToken);
    }
}