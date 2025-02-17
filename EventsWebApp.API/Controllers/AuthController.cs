using EventsWebApp.API.Extensions;
using EventsWebApp.Application.DTOs;
using EventsWebApp.Application.UseCases.Auth.ConfirmEmail;
using EventsWebApp.Application.UseCases.Auth.CreateToken;
using EventsWebApp.Application.UseCases.Auth.LoginUser;
using EventsWebApp.Application.UseCases.Auth.RegisterUser;
using EventsWebApp.Application.UseCases.Auth.SendEmailConfirmationToken;
using EventsWebApp.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EventsWebApp.API.Controllers;

[Consumes("application/json")]
[Route("api/auth")]
[ApiController]
public class AuthController(ISender sender, UserManager<User> userManager) : ControllerBase
{
	private readonly ISender _sender = sender;
	private readonly UserManager<User> _userManager = userManager;

	[HttpPost(Name = "SignUp")]
	public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistration)
	{
		var baseResult = await _sender.Send(new RegisterUserUseCase(userForRegistration));

		var (result, user) = baseResult.GetResult<(IdentityResult, User)>();

		if (!result.Succeeded)
		{
			foreach (var error in result.Errors)
			{
				ModelState.TryAddModelError(error.Code, error.Description);
			}
			return BadRequest(ModelState);
		}

		var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
		var confirmationLink = Url.Action(nameof(ConfirmEmail), "Auth", new { token, email = user.Email }, Request.Scheme);
		await _sender.Send(new SendEmailConfirmationTokenUseCase(confirmationLink, user.Email));

		return StatusCode(201);
	}

	[HttpPost("login", Name = "SignIn")]
	public async Task<IActionResult> LoginUser([FromBody] UserForAuthenticationDto userForAuth)
	{
		var baseResult = await _sender.Send(new LoginUserUseCase(userForAuth));

		var (isValid, user) = baseResult.GetResult<(bool, User?)>();

		if (!isValid || user == null)
			return Unauthorized("Invalid username or password.");

		var tokenDtoBaseResult = await _sender.Send(new CreateTokenUseCase(user, PopulateExp: true));
		var tokenDto = tokenDtoBaseResult.GetResult<TokenDto>();

		return Ok(tokenDto);
	}

	[HttpGet("confirm-email")]
	public async Task<IActionResult> ConfirmEmail(string token, string email)
	{
		var baseResult = await _sender.Send(new ConfirmEmailUseCase(email, token));
		var result = baseResult.GetResult<IdentityResult>();

		if (!result.Succeeded)
		{
			foreach (var error in result.Errors)
			{
				ModelState.TryAddModelError(error.Code, error.Description);
			}
			return BadRequest(ModelState);
		}

		return Ok(new { Message = "Email verified successfully." });
	}
}
