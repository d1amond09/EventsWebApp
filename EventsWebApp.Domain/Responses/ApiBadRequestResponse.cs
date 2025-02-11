namespace EventsWebApp.Domain.Responses;

public class ApiBadRequestResponse(string message) : ApiBaseResponse(false)
{
	public string Message { get; set; } = message;
}

