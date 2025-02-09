using System.Reflection.Metadata;
using EventsWebApp.Application;
using EventsWebApp.Application.DTOs;
using EventsWebApp.Domain.Contracts.Persistence;
using EventsWebApp.Domain.Contracts.Services;
using EventsWebApp.Domain.Entities;
using EventsWebApp.Infrastructure.Persistence;
using EventsWebApp.Infrastructure.Persistence.Repositories;
using EventsWebApp.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NLog;

namespace EventsWebApp.API.Extensions;

public static class BuilderServiceCollectionExtensions
{
	public static WebApplicationBuilder AddDataBase(this WebApplicationBuilder builder)
	{
		builder.Services.AddDbContext<AppDbContext>(opts =>
			opts.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"), b =>
			{
				b.MigrationsAssembly("EventsWebApp.Infrastructure");
				b.EnableRetryOnFailure();
			})
		);
		return builder;
	}

	public static WebApplicationBuilder AddApplicationServices(this WebApplicationBuilder builder)
	{
		builder.Services.AddControllers();
		builder.Services.AddOpenApi();
		builder.Services.AddCors(options =>
		{
			options.AddPolicy("CorsPolicy", builder =>
			builder.AllowAnyOrigin()
			//builder.WithOrigins("http://localhost:5000")
			.AllowAnyMethod()
			.AllowAnyHeader()
			.WithExposedHeaders("X-Pagination"));
		});

		builder.Services.AddAutoMapper(x => x.AddProfile(new MappingProfile()));
		builder.Services.AddMediatR(cfg =>
			cfg.RegisterServicesFromAssembly(typeof(MappingProfile).Assembly));

		builder.Services.AddScoped<IRepositoryManager, RepositoryManager>();
		return builder;
	}

	public static WebApplicationBuilder AddInfrastructureServices(this WebApplicationBuilder builder)
	{
		LogManager.Setup().LoadConfigurationFromFile("nlog.config", true);

		builder.Services.AddSingleton<ILoggingService, LoggingService>();

		builder.Services.AddScoped<IDataShapeService<EventDto>, DataShapeService<EventDto>>();
		builder.Services.AddScoped<IDataShapeService<ParticipantDto>, DataShapeService<ParticipantDto>>();
		builder.Services.AddScoped<IDataShapeService<UserDto>, DataShapeService<UserDto>>();

		return builder;
	}

	public static WebApplicationBuilder AddConfigurationIdentity(this WebApplicationBuilder builder)
	{
		var idBuilder = builder.Services.AddIdentity<User, Role>(o =>
		{
			o.Password.RequireDigit = true;
			o.Password.RequireLowercase = true;
			o.Password.RequireUppercase = true;
			o.Password.RequireNonAlphanumeric = true;
			o.Password.RequiredLength = 8;
			o.User.RequireUniqueEmail = true;
			o.SignIn.RequireConfirmedEmail = true;
			o.Tokens.EmailConfirmationTokenProvider = "emailconfirmation";
		})
		.AddEntityFrameworkStores<AppDbContext>()
		.AddDefaultTokenProviders();

		builder.Services.Configure<DataProtectionTokenProviderOptions>(opt =>
			opt.TokenLifespan = TimeSpan.FromHours(2));

		return builder;
	}
}
