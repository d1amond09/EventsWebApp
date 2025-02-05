using EventsWebApp.Domain.Entities;
using EventsWebApp.Infrastructure.Persistence.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EventsWebApp.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<User, Role, Guid>(options)
{
	public DbSet<Event> Event { get; set; }
	public DbSet<Participant> Participants { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.ApplyConfiguration(new RoleConfiguration());
		modelBuilder.ApplyConfiguration(new UserConfiguration());
		modelBuilder.ApplyConfiguration(new ParticipantConfiguration());
		modelBuilder.ApplyConfiguration(new EventConfiguration());
	}
}
