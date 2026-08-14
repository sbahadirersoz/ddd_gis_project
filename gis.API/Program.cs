using System.Runtime.CompilerServices;
using gis.ApplicationLayer.DependencyInjection;
using gis.InfrastructureLayer.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.InjectServicesFromInfrastructure();
builder.Services.MediatRInjection();
builder.Services.InjectDB(builder.Configuration);
var app = builder.Build();
app.MapControllers();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


app.Run();

