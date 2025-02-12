namespace EventsWebApp.Domain.Responses;

public sealed class ApiOkResponse<TResult>(TResult result) : ApiBaseResultResponse<TResult>(result, true)
{
}

