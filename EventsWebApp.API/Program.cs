using EventsWebApp.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder
    .AddApplicationServices()
    .AddInfrastructureServices()
	.AddConfigurationIdentity()
    .AddDataBase();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app
    .AddMiddlewares()
    .AddAppServices()
	.AddBaseDependencies();


app.Run();
