using EventsWebApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventsWebApp.Infrastructure.Persistence.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
	public void Configure(EntityTypeBuilder<Role> builder)
	{
		builder.HasData(
			new Role
			{
				Id = new Guid("11111111-49b6-410c-bc78-2d54a9991870"),
				Name = "Administrator",
				NormalizedName = "ADMINISTRATOR"
			},
			new Role
			{
				Id = new Guid("11111112-49b6-410c-bc78-2d54a9991870"),
				Name = "User",
				NormalizedName = "USER"
			}
		);
	}
}
