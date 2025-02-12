namespace EventsWebApp.Domain.Entities;

public class Participant
{
	public Guid Id { get; set; }
	public Guid EventId { get; set; }
	public virtual Event? Event { get; set; }
	public Guid UserId { get; set; }
	public virtual User? User { get; set; }
	public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
}
