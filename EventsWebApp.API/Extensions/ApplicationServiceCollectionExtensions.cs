using EventsWebApp.API.Middlewares;
using EventsWebApp.Domain.Contracts.Services;
using Microsoft.AspNetCore.HttpOverrides;

namespace EventsWebApp.API.Extensions;

public static class ApplicationServiceCollectionExtensions
{
	public static WebApplication AddMiddlewares(this WebApplication app)
	{
		app.UseMiddleware<ExceptionMiddleware>();
		return app;
	}

	public static WebApplication AddAppServices(this WebApplication app)
	{
		var logger = app.Services.GetRequiredService<ILoggingService>();
		app.UseCors("CorsPolicy");

		app.UseHttpsRedirection();
		app.UseStaticFiles();
		app.UseForwardedHeaders(new ForwardedHeadersOptions
		{
			ForwardedHeaders = ForwardedHeaders.All
		});
		app.UseRouting();
		app.UseAuthentication();
		app.UseAuthorization();
		return app;
	}

	public static WebApplication AddBaseDependencies(this WebApplication app)
	{

		app.MapControllers();
		return app;
	}
}
