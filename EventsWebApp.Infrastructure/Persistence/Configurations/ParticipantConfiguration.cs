using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using EventsWebApp.Domain.Entities;

namespace EventsWebApp.Infrastructure.Persistence.Configurations;

public class ParticipantConfiguration : IEntityTypeConfiguration<Participant>
{
	public void Configure(EntityTypeBuilder<Participant> builder)
	{
		builder.HasData(
		[

		]);
	}
}
