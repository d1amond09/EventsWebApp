using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EventsWebApp.Domain.ConfigurationModels;
using EventsWebApp.Domain.Contracts.Persistence;
using EventsWebApp.Domain.Entities;
using EventsWebApp.Domain.Responses;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace EventsWebApp.Application.UseCases.Auth.RefreshToken;

public class RefreshTokenHandler(IOptionsMonitor<JwtConfiguration> configuration, IConfiguration config, IRepositoryManager rep) :
	IRequestHandler<RefreshTokenUseCase, ApiBaseResponse>
{
	private readonly IOptionsMonitor<JwtConfiguration> _configuration = configuration;
	private readonly IConfiguration _config = config;
	private readonly IRepositoryManager _rep = rep;
	public JwtConfiguration JwtConfiguration => _configuration.Get("JwtSettings");

	public async Task<ApiBaseResponse> Handle(RefreshTokenUseCase request, CancellationToken cancellationToken)
	{
		var principal = GetPrincipalFromExpiredToken(request.TokenDto.AccessToken);
		var user = await _rep.Users.GetByUserNameAsync(principal.Identity?.Name!);

		if (user == null ||
			user.RefreshToken != request.TokenDto.RefreshToken ||
			user.RefreshTokenExpiryTime <= DateTime.Now)
			return new ApiBadRequestResponse("");

		return new ApiOkResponse<User>(user);
	}

	public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
	{
		var secretValue = _config["SECRET"];
		if (string.IsNullOrEmpty(secretValue))
		{
			throw new InvalidOperationException("The SECRET configuration value is missing.");
		}

		var tokenValidationParameters = new TokenValidationParameters
		{
			ValidateAudience = true,
			ValidateIssuer = true,
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = new SymmetricSecurityKey(
				Encoding.UTF8.GetBytes(secretValue)),
			ValidateLifetime = true,
			ValidIssuer = JwtConfiguration.ValidIssuer,
			ValidAudience = JwtConfiguration.ValidAudience
		};

		var tokenHandler = new JwtSecurityTokenHandler();
		var principal = tokenHandler
			.ValidateToken(
				token,
				tokenValidationParameters,
				out SecurityToken securityToken);

		if (securityToken is not JwtSecurityToken jwtSecurityToken ||
			!jwtSecurityToken.Header.Alg.Equals(
				SecurityAlgorithms.HmacSha256,
				StringComparison.InvariantCultureIgnoreCase)
			)
		{
			throw new SecurityTokenException("Invalid token");
		}

		return principal;
	}
}