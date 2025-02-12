using EventsWebApp.Domain.Entities;

namespace EventsWebApp.Application.DTOs;

public record EventDto
{
	public Guid Id { get; init; }
	public string Name { get; init; } = string.Empty;
	public string? Description { get; init; } = string.Empty;
	public DateTime DateTime { get; init; } = DateTime.UtcNow;
	public string Location { get; init; } = string.Empty;
	public string Category { get; init; } = string.Empty;
	public int MaxCountParticipants { get; init; }
	public byte[]? Image { get; init; } = null;
	public ICollection<ParticipantDto> Participants { get; init; } = [];
}

public record EventForManipulationDto
{
	public string Name { get; init; } = string.Empty;
	public string? Description { get; init; } = string.Empty;
	public DateTime DateTime { get; init; } = DateTime.UtcNow;
	public string Location { get; init; } = string.Empty;
	public string Category { get; init; } = string.Empty;
	public int MaxCountParticipants { get; init; }
	public byte[]? Image { get; init; } = null;
}

public record EventForUpdateDto : EventForManipulationDto;
public record EventForCreationDto : EventForManipulationDto;
