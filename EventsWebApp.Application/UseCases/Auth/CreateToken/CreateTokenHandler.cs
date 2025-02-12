using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using EventsWebApp.Application.DTOs;
using EventsWebApp.Application.UseCases.Auth.RegisterUser;
using EventsWebApp.Application.UseCases.Events.CreateEvent;
using EventsWebApp.Domain.ConfigurationModels;
using EventsWebApp.Domain.Contracts.Persistence;
using EventsWebApp.Domain.Entities;
using EventsWebApp.Domain.Responses;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace EventsWebApp.Application.UseCases.Auth.CreateToken;

public class CreateTokenHandler : IRequestHandler<CreateTokenUseCase, ApiBaseResponse>
{

	private readonly IOptionsMonitor<JwtConfiguration> _configuration;
	private readonly JwtConfiguration _jwtConfiguration;
	private readonly IRepositoryManager _rep;
	private readonly UserManager<User> _userManager;
	private readonly IConfiguration _config;

	public CreateTokenHandler(
		UserManager<User> userManager,
		IOptionsMonitor<JwtConfiguration> configuration,
		IRepositoryManager rep,
		IConfiguration config)
	{
		_rep = rep;
		_userManager = userManager;
		_configuration = configuration;
		_jwtConfiguration = _configuration.Get("JwtSettings");
		_config = config;
	}

	public async Task<ApiBaseResponse> Handle(CreateTokenUseCase request, CancellationToken cancellationToken)
	{
		var signingCredentials = GetSigningCredentials();
		var claims = await GetClaims(request.User);
		var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

		var refreshToken = GenerateRefreshToken();

		request.User.RefreshToken = refreshToken;

		if (request.PopulateExp)
			request.User.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

		await _rep.Users.UpdateAsync(request.User);

		var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
		var tokenDto = new TokenDto(accessToken, refreshToken);

		return new ApiOkResponse<TokenDto>(tokenDto);
	}

	private string GenerateRefreshToken()
	{
		var randomNumber = new byte[32];
		using var rng = RandomNumberGenerator.Create();
		rng.GetBytes(randomNumber);
		return Convert.ToBase64String(randomNumber);
	}

	private JwtSecurityToken GenerateTokenOptions(
		SigningCredentials signingCredentials,
		List<Claim> claims)
	{
		var tokenOptions = new JwtSecurityToken(
			issuer: _jwtConfiguration.ValidIssuer,
			audience: _jwtConfiguration.ValidAudience,
			claims: claims,
			expires: DateTime.Now
				.AddMinutes(Convert.ToDouble(_jwtConfiguration.Expires)),
			signingCredentials: signingCredentials
		);

		return tokenOptions;
	}

	private SigningCredentials GetSigningCredentials()
	{
		var secretValue = _config["SECRET"];
		if (string.IsNullOrEmpty(secretValue))
		{
			throw new InvalidOperationException("The SECRET configuration value is missing.");
		}

		var key = Encoding.UTF8.GetBytes(secretValue);
		var secret = new SymmetricSecurityKey(key);
		return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
	}

	private async Task<List<Claim>> GetClaims(User user)
	{
		var claims = new List<Claim>
		{
			new (ClaimTypes.Name, user.UserName),
			new (ClaimTypes.NameIdentifier, user.Id.ToString())
		};

		var roles = await _rep.Users.GetRolesAsync(user);
		foreach (var role in roles)
		{
			claims.Add(new Claim(ClaimTypes.Role, role));
		}

		return claims;
	}
}
