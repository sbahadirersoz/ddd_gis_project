using gis.ApplicationLayer.DependencyInjection;
using gis.InfrastructureLayer.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddControllers();
var app = builder.Build();
app.MapControllers();
builder.Services.InjectServices();
builder.Services.MediatRInjection();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


app.Run();

