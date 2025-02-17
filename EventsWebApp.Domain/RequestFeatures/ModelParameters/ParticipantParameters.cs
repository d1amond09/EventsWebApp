namespace EventsWebApp.Domain.RequestFeatures.ModelParameters;

public class ParticipantParameters : RequestParameters
{
	public DateTime MinRegisteredAt { get; set; } = DateTime.MinValue;
	public DateTime MaxRegisteredAt { get; set; } = DateTime.MaxValue;
	public bool NotValidRegisteredAtRange => MaxRegisteredAt <= MinRegisteredAt;
	public ParticipantParameters()
	{
		OrderBy = "registeredAt";
	}
}
