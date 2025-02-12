namespace EventsWebApp.Domain.Responses;

public class ApiNotFoundResponse(string message) : ApiBaseResponse(false)
{
	public ApiNotFoundResponse(string name, Guid id) : 
		this($"{name} with id:{id} not found!") { }

	public string Message { get; set; } = message;
}
