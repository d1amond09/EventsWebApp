using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EventsWebApp.Infrastructure.Migrations
{
	/// <inheritdoc />
	public partial class AddModelsDB : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "AspNetRoles",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uuid", nullable: false),
					Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
					NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
					ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AspNetRoles", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "AspNetUsers",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uuid", nullable: false),
					FirstName = table.Column<string>(type: "text", nullable: true),
					LastName = table.Column<string>(type: "text", nullable: true),
					RefreshToken = table.Column<string>(type: "text", nullable: true),
					RefreshTokenExpiryTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
					BirthDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
					UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
					NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
					Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
					NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
					EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
					PasswordHash = table.Column<string>(type: "text", nullable: true),
					SecurityStamp = table.Column<string>(type: "text", nullable: true),
					ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
					PhoneNumber = table.Column<string>(type: "text", nullable: true),
					PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
					TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
					LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
					LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
					AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AspNetUsers", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "Event",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uuid", nullable: false),
					Name = table.Column<string>(type: "text", nullable: false),
					Description = table.Column<string>(type: "text", nullable: true),
					DateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
					Location = table.Column<string>(type: "text", nullable: false),
					Category = table.Column<string>(type: "text", nullable: false),
					MaxCountParticipants = table.Column<int>(type: "integer", nullable: false),
					Image = table.Column<byte[]>(type: "bytea", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Event", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "AspNetRoleClaims",
				columns: table => new
				{
					Id = table.Column<int>(type: "integer", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					RoleId = table.Column<Guid>(type: "uuid", nullable: false),
					ClaimType = table.Column<string>(type: "text", nullable: true),
					ClaimValue = table.Column<string>(type: "text", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
					table.ForeignKey(
						name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
						column: x => x.RoleId,
						principalTable: "AspNetRoles",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "AspNetUserClaims",
				columns: table => new
				{
					Id = table.Column<int>(type: "integer", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					UserId = table.Column<Guid>(type: "uuid", nullable: false),
					ClaimType = table.Column<string>(type: "text", nullable: true),
					ClaimValue = table.Column<string>(type: "text", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
					table.ForeignKey(
						name: "FK_AspNetUserClaims_AspNetUsers_UserId",
						column: x => x.UserId,
						principalTable: "AspNetUsers",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "AspNetUserLogins",
				columns: table => new
				{
					LoginProvider = table.Column<string>(type: "text", nullable: false),
					ProviderKey = table.Column<string>(type: "text", nullable: false),
					ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
					UserId = table.Column<Guid>(type: "uuid", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
					table.ForeignKey(
						name: "FK_AspNetUserLogins_AspNetUsers_UserId",
						column: x => x.UserId,
						principalTable: "AspNetUsers",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "AspNetUserRoles",
				columns: table => new
				{
					UserId = table.Column<Guid>(type: "uuid", nullable: false),
					RoleId = table.Column<Guid>(type: "uuid", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
					table.ForeignKey(
						name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
						column: x => x.RoleId,
						principalTable: "AspNetRoles",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_AspNetUserRoles_AspNetUsers_UserId",
						column: x => x.UserId,
						principalTable: "AspNetUsers",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "AspNetUserTokens",
				columns: table => new
				{
					UserId = table.Column<Guid>(type: "uuid", nullable: false),
					LoginProvider = table.Column<string>(type: "text", nullable: false),
					Name = table.Column<string>(type: "text", nullable: false),
					Value = table.Column<string>(type: "text", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
					table.ForeignKey(
						name: "FK_AspNetUserTokens_AspNetUsers_UserId",
						column: x => x.UserId,
						principalTable: "AspNetUsers",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "Participants",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uuid", nullable: false),
					EventId = table.Column<Guid>(type: "uuid", nullable: false),
					UserId = table.Column<Guid>(type: "uuid", nullable: false),
					RegisteredAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Participants", x => x.Id);
					table.ForeignKey(
						name: "FK_Participants_AspNetUsers_UserId",
						column: x => x.UserId,
						principalTable: "AspNetUsers",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_Participants_Event_EventId",
						column: x => x.EventId,
						principalTable: "Event",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.InsertData(
				table: "AspNetRoles",
				columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
				values: new object[,]
				{
					{ new Guid("11111111-49b6-410c-bc78-2d54a9991870"), null, "Administrator", "ADMINISTRATOR" },
					{ new Guid("11111112-49b6-410c-bc78-2d54a9991870"), null, "User", "USER" }
				});

			migrationBuilder.InsertData(
				table: "Event",
				columns: new[] { "Id", "Category", "DateTime", "Description", "Image", "Location", "MaxCountParticipants", "Name" },
				values: new object[,]
				{
					{ new Guid("c9d4c011-49b6-410c-bc78-2d54a9991870"), "Category2", new DateTime(2025, 2, 13, 18, 30, 0, 0, DateTimeKind.Utc), "", null, "Belarus, Gomel", 16, "Event 2" },
					{ new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"), "Category1", new DateTime(2025, 2, 12, 12, 0, 0, 0, DateTimeKind.Utc), "", null, "Belarus, Gomel", 2, "Event 1" }
				});

			migrationBuilder.CreateIndex(
				name: "IX_AspNetRoleClaims_RoleId",
				table: "AspNetRoleClaims",
				column: "RoleId");

			migrationBuilder.CreateIndex(
				name: "RoleNameIndex",
				table: "AspNetRoles",
				column: "NormalizedName",
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_AspNetUserClaims_UserId",
				table: "AspNetUserClaims",
				column: "UserId");

			migrationBuilder.CreateIndex(
				name: "IX_AspNetUserLogins_UserId",
				table: "AspNetUserLogins",
				column: "UserId");

			migrationBuilder.CreateIndex(
				name: "IX_AspNetUserRoles_RoleId",
				table: "AspNetUserRoles",
				column: "RoleId");

			migrationBuilder.CreateIndex(
				name: "EmailIndex",
				table: "AspNetUsers",
				column: "NormalizedEmail");

			migrationBuilder.CreateIndex(
				name: "UserNameIndex",
				table: "AspNetUsers",
				column: "NormalizedUserName",
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_Participants_EventId",
				table: "Participants",
				column: "EventId");

			migrationBuilder.CreateIndex(
				name: "IX_Participants_UserId",
				table: "Participants",
				column: "UserId");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "AspNetRoleClaims");

			migrationBuilder.DropTable(
				name: "AspNetUserClaims");

			migrationBuilder.DropTable(
				name: "AspNetUserLogins");

			migrationBuilder.DropTable(
				name: "AspNetUserRoles");

			migrationBuilder.DropTable(
				name: "AspNetUserTokens");

			migrationBuilder.DropTable(
				name: "Participants");

			migrationBuilder.DropTable(
				name: "AspNetRoles");

			migrationBuilder.DropTable(
				name: "AspNetUsers");

			migrationBuilder.DropTable(
				name: "Event");
		}
	}
}
