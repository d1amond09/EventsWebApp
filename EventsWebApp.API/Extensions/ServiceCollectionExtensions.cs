using EventsWebApp.Domain.Contracts;
using EventsWebApp.Domain.Entities;
using EventsWebApp.Infrastructure.Persistence;
using EventsWebApp.Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EventsWebApp.API.Extensions;

public static class ServiceCollectionExtensions
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
		builder.Services.AddScoped<IRepositoryManager, RepositoryManager>();

		builder.Services.AddCors(options =>
		{
			options.AddPolicy("CorsPolicy", builder =>
			builder.AllowAnyOrigin()
			//builder.WithOrigins("http://localhost:5000")
			.AllowAnyMethod()
			.AllowAnyHeader()
			.WithExposedHeaders("X-Pagination"));
		});

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
