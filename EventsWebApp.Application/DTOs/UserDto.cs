using System.ComponentModel.DataAnnotations;
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

public record UserForRegistrationDto
{
	public string? FirstName { get; init; }
	public string? LastName { get; init; }
	[Required(ErrorMessage = "Username is required")]
	public string? UserName { get; init; }

	[DataType(DataType.Password)]
	[Required(ErrorMessage = "Password is required")]
	public string? Password { get; init; }

	[DataType(DataType.Password)]
	[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
	public string? ConfirmPassword { get; set; }
	public string? Confirm { get; init; }

	[Required(ErrorMessage = "Email is required")]
	[EmailAddress]
	public string? Email { get; init; }
	public string? PhoneNumber { get; init; }
	public ICollection<string>? Roles { get; init; }
}

public record UserForAuthenticationDto
{
	[Required(ErrorMessage = "UserName is required")]
	public string? UserName { get; init; }
	[Required(ErrorMessage = "Password name is required")]
	public string? Password { get; init; }
}

public record UserForUpdateDto
{
	public string? FirstName { get; init; }
	public string? LastName { get; init; }
	public string? UserName { get; init; }
	public string? Password { get; init; }
	public string? Email { get; init; }
	public string? PhoneNumber { get; init; }
}