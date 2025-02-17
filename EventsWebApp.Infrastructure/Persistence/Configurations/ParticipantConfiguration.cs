using EventsWebApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
