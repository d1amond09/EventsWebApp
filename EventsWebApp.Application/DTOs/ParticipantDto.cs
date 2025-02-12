using EventsWebApp.Domain.Entities;

namespace EventsWebApp.Application.DTOs;

public record ParticipantDto
{
	public Guid Id { get; init; }
	public Guid EventId { get; init; }
	public Guid UserId { get; init; }
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
	public DateTime BirthDate { get; init; }
	public DateTime RegisteredAt { get; init; } = DateTime.UtcNow;
	public string? Email { get; set; }
}
