using EventsWebApp.API.Extensions;
using EventsWebApp.Application.DTOs;
using EventsWebApp.Application.UseCases.Auth.CreateToken;
using EventsWebApp.Application.UseCases.Auth.RefreshToken;
using EventsWebApp.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EventsWebApp.API.Controllers;

[Consumes("application/json")]
[Route("api/token")]
[ApiController]
public class TokenController(ISender sender) : ControllerBase
{
	private readonly ISender _sender = sender;

	[HttpPost("refresh")]
	public async Task<IActionResult> Refresh([FromBody] TokenDto tokenDto)
	{
		var baseResultRefresh = await _sender.Send(new RefreshTokenUseCase(tokenDto));
		var user = baseResultRefresh.GetResult<User>();

		var baseResult = await _sender.Send(new CreateTokenUseCase(user, PopulateExp: false));
		var tokenDtoToReturn = baseResult.GetResult<TokenDto>();

		return Ok(tokenDtoToReturn);
	}
}
