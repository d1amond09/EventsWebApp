namespace EventsWebApp.Domain.Responses;

public class ApiNotValidRangeBadRequestResponse(string fieldName) :
	ApiBadRequestResponse($"Max {fieldName} can't be less than min {fieldName}")
{ }
