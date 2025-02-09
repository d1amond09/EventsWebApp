using EventsWebApp.Domain.Entities;

namespace EventsWebApp.Application.DTOs;

public record UserDto
{
	public Guid Id { get; init; }
	public string? UserName { get; init; }
	public string? FirstName { get; init; }
	public string? LastName { get; init; }
	public string? Email { get; init; }
	public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
	public virtual ICollection<ParticipantDto> Participants { get; init; } = [];
}

public record UserForManipulationDto
{
	public string? UserName { get; init; }
	public string? FirstName { get; init; }
	public string? LastName { get; init; }
	public string? Email { get; init; }
	public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
	public virtual ICollection<ParticipantDto> Participants { get; init; } = [];
}

public record UserForUpdateDto : UserForManipulationDto;
public record UserForCreationDto : UserForManipulationDto;