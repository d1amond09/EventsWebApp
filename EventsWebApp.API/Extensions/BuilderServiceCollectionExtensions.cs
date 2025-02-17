using EventsWebApp.Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using EventsWebApp.Domain.Contracts.Persistence;
using EventsWebApp.Domain.ConfigurationModels;
using EventsWebApp.Infrastructure.Persistence;
using EventsWebApp.Domain.Contracts.Services;
using EventsWebApp.API.CustomTokenProviders;
using EventsWebApp.Infrastructure.Services;
using EventsWebApp.Application.Behaviors;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using EventsWebApp.Application.DTOs;
using Microsoft.EntityFrameworkCore;
using EventsWebApp.Domain.Entities;
using EventsWebApp.Application;
using FluentValidation;
using System.Text;
using MediatR;
using NLog;

namespace EventsWebApp.API.Extensions;

public static class BuilderServiceCollectionExtensions
{
	public static WebApplicationBuilder AddDataBase(this WebApplicationBuilder builder)
	{
		builder.Services.AddDbContext<AppDbContext>(opts =>
			opts.UseNpgsql(builder.Configuration.GetConnectionString("DockerConnection"), b =>
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
		
		builder.Services.AddAuthentication();

		builder.Services.AddAutoMapper(x => x.AddProfile(new MappingProfile()));
		builder.Services.AddMediatR(cfg =>
			cfg.RegisterServicesFromAssembly(typeof(MappingProfile).Assembly));

		builder.Services.AddScoped<IRepositoryManager, RepositoryManager>();

		builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
		builder.Services.AddValidatorsFromAssembly(typeof(MappingProfile).Assembly);

		return builder;
	}

	public static WebApplicationBuilder AddJwtConfig(this WebApplicationBuilder builder)
	{
		var jwtConfiguration = new JwtConfiguration();
		builder.Configuration.Bind(jwtConfiguration.Section, jwtConfiguration);

		var secretKey = builder.Configuration.GetValue<string>("SECRET");
		ArgumentNullException.ThrowIfNull(secretKey);

		builder.Services.AddAuthentication(opt =>
		{
			opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
		})
			.AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,

					ValidIssuer = jwtConfiguration.ValidIssuer,
					ValidAudience = jwtConfiguration.ValidAudience,
					IssuerSigningKey = new
						SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
				};
			});

		builder.Services.Configure<JwtConfiguration>("JwtSettings", builder.Configuration.GetSection("JwtSettings"));
		return builder;
	}

	public static WebApplicationBuilder AddInfrastructureServices(this WebApplicationBuilder builder)
	{
		LogManager.Setup().LoadConfigurationFromFile("nlog.config", true);

		builder.Services.AddSingleton<ILoggingService, LoggingService>();

		builder.Services.AddScoped<IDataShapeService<EventDto>, DataShapeService<EventDto>>();
		builder.Services.AddScoped<IDataShapeService<ParticipantDto>, DataShapeService<ParticipantDto>>();
		builder.Services.AddScoped<IDataShapeService<UserDto>, DataShapeService<UserDto>>();

		EmailConfiguration? emailConfig = builder.Configuration
			.GetSection("EmailConfiguration")
			.Get<EmailConfiguration>();
		ArgumentNullException.ThrowIfNull(emailConfig);
		builder.Services.AddSingleton(emailConfig);

		builder.Services.AddScoped<IEmailSendService, EmailSendService>();

		builder.Services.Configure<FormOptions>(o => {
			o.ValueLengthLimit = int.MaxValue;
			o.MultipartBodyLengthLimit = int.MaxValue;
			o.MemoryBufferThreshold = int.MaxValue;
		});

		return builder;
	}

	public static WebApplicationBuilder AddConfigIdentity(this WebApplicationBuilder builder)
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
		.AddDefaultTokenProviders()
		 .AddTokenProvider<EmailConfirmationTokenProvider<User>>("emailconfirmation");

		builder.Services.Configure<DataProtectionTokenProviderOptions>(opt =>
			opt.TokenLifespan = TimeSpan.FromHours(2));

		builder.Services.Configure<EmailConfirmationTokenProviderOptions>(opt =>
			opt.TokenLifespan = TimeSpan.FromDays(3));

		return builder;
	}
}
