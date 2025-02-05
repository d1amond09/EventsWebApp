using System.Text.Json;

namespace EventsWebApp.API.Middlewares;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
	private readonly RequestDelegate _next = next;
	private readonly ILogger<ExceptionMiddleware> _logger = logger;

	public async Task InvokeAsync(HttpContext context)
	{
		try
		{
			await _next(context);
		}
		catch (Exception ex)
		{
			_logger.LogError($"Произошла ошибка: {ex.Message}");
			await HandleExceptionAsync(context, ex);
		}
	}

	private static Task HandleExceptionAsync(HttpContext context, Exception exception)
	{
		context.Response.ContentType = "application/json";
		context.Response.StatusCode = exception switch
		{
			ArgumentException _ => StatusCodes.Status400BadRequest,
			UnauthorizedAccessException _ => StatusCodes.Status401Unauthorized,
			InvalidOperationException _ => StatusCodes.Status403Forbidden,
			KeyNotFoundException _ => StatusCodes.Status404NotFound,
			_ => StatusCodes.Status500InternalServerError,
		};

		var response = new
		{
			context.Response.StatusCode,
			Message = "Произошла ошибка. Попробуйте позже.",
			Details = exception.Message
		};

		return context.Response.WriteAsync(JsonSerializer.Serialize(response));
	}
}
