using EventsWebApp.API.Extensions;
using EventsWebApp.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder
    .AddApplicationServices()
    .AddConfigurationIdentity()
    .AddDataBase();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
