using System.Diagnostics;
using gis.Domain.ResultPattern;
using MediatR;
using Microsoft.Extensions.Logging;

namespace gis.ApplicationLayer.Pipelines.Validators;

public class LoggingBehavior<TRequest,TResponse>:IPipelineBehavior<TRequest,TResponse>
where TResponse : Result
{
    private readonly ILogger<LoggingBehavior<TRequest,TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // TRequest'in tip ismini alıyoruz
        var requestName = typeof(TRequest).Name;

        _logger.LogDebug("İşlem başlatıldı: {RequestName}", requestName);

        var timer = Stopwatch.StartNew();
        try
        {
            var response = await next();
        
            timer.Stop();
            _logger.LogDebug("İşlem başarıyla tamamlandı: {RequestName} ({ElapsedMilliseconds} ms)", 
                requestName, timer.ElapsedMilliseconds);

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogDebug(ex, "İşlem sırasında hata oluştu: {RequestName}", requestName);
            throw;
        }
    }

}