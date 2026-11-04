using System.Runtime.CompilerServices;
using Gis.Application.DependencyInjection;
using Gis.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.InjectServicesFromInfrastructure();
builder.Services.MediatRInjection();
builder.Services.InjectDB(builder.Configuration);
builder.Services.FluentValidationInjection();
var app = builder.Build();
app.MapControllers();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


app.Run();

