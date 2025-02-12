namespace EventsWebApp.Domain.Responses;

public abstract class ApiBaseResultResponse<TResult>(TResult result, bool success) : ApiBaseResponse(success)
{
	public TResult Result { get; set; } = result;
}
