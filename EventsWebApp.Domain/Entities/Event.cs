namespace EventsWebApp.Domain.Entities;

public class Event
{
	public Guid Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string? Description { get; set; } = string.Empty;
	public DateTime DateTime { get; set; } = DateTime.UtcNow;
	public string Location { get; set; } = string.Empty;
	public string Category { get; set; } = string.Empty;
	public int MaxCountParticipants { get; set; }
	public string? Image { get; set; } = null;
	public virtual ICollection<Participant> Participants { get; set; } = [];
}
