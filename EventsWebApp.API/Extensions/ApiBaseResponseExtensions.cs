using EventsWebApp.Domain.Responses;

namespace EventsWebApp.API.Extensions;

public static class ApiBaseResponseExtensions
{
	public static TResultType GetResult<TResultType>(this ApiBaseResponse apiBaseResponse) =>
		((ApiBaseResultResponse<TResultType>)apiBaseResponse).Result;

}
