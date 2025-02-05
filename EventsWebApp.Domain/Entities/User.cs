using Microsoft.AspNetCore.Identity;

namespace EventsWebApp.Domain.Entities;

public class User : IdentityUser<Guid>
{
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
	public string? RefreshToken { get; set; }
	public DateTime RefreshTokenExpiryTime { get; set; }
	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
	public virtual ICollection<Participant> Participants { get; set; } = [];
}
