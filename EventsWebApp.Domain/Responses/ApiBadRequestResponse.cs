namespace EventsWebApp.Domain.Responses;

public class ApiBadRequestResponse(string message) : ApiBaseResultResponse<string>(message, false)
{
}

