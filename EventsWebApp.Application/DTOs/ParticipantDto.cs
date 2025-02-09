using EventsWebApp.Domain.Entities;

namespace EventsWebApp.Application.DTOs;

public record ParticipantDto
{
	public Guid Id { get; init; }
	public Guid EventId { get; init; }
	public EventDto? Event { get; init; }
	public Guid UserId { get; init; }
	public UserDto? User { get; init; }
	public DateTime RegisteredAt { get; init; } = DateTime.UtcNow;
}

public record ParticipantForManipulationDto
{
	public Guid EventId { get; init; }
	public EventDto? Event { get; init; }
	public Guid UserId { get; init; }
	public UserDto? User { get; init; }
	public DateTime RegisteredAt { get; init; } = DateTime.UtcNow;
}

public record ParticipantForUpdateDto : ParticipantForManipulationDto;
public record ParticipantForCreationDto : ParticipantForManipulationDto;
