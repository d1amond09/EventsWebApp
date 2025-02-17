using EventsWebApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventsWebApp.Infrastructure.Persistence.Configurations;

public class EventConfiguration : IEntityTypeConfiguration<Event>
{
	public void Configure(EntityTypeBuilder<Event> builder)
	{
		builder.Property(e => e.Id)
		   .IsRequired();

		builder.Property(e => e.Name)
			.IsRequired()
			.HasMaxLength(100);

		builder.Property(e => e.Category)
			.IsRequired()
			.HasMaxLength(255);

		builder.Property(e => e.Location)
			.HasMaxLength(255);

		builder.Property(e => e.Description)
			.IsRequired()
			.HasMaxLength(500);

		builder.HasData(
		[
			new Event
			{
				Id = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"),
				Name = "Event 1",
				DateTime = new DateTime(2025,2,12,12,0,0, DateTimeKind.Utc),
				Category = "Category1",
				Image = null,
				Location = "Belarus, Gomel",
				MaxCountParticipants = 2,
			},
			new Event
			{
				Id = new Guid("c9d4c011-49b6-410c-bc78-2d54a9991870"),
				Name = "Event 2",
				DateTime = new DateTime(2025,2,13,18,30,0, DateTimeKind.Utc),
				Category = "Category2",
				Image = null,
				Location = "Belarus, Gomel",
				MaxCountParticipants = 16,
			},
		]);
	}
}
