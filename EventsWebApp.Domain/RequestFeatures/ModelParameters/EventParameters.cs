using EventsWebApp.Domain.Entities;

namespace EventsWebApp.Domain.RequestFeatures.ModelParameters;

public class EventParameters : RequestParameters
{
	public DateTime MinDateTime { get; set; } = DateTime.MinValue;
	public DateTime MaxDateTime { get; set; } = DateTime.MaxValue;
	public bool NotValidDateTimeRange => MaxDateTime <= MinDateTime;
	public string Name { get; set; } = string.Empty;
	public string Location { get; set; } = string.Empty;
	public string Category { get; set; } = string.Empty;
	public EventParameters()
	{
		OrderBy = "name";
	}
}
