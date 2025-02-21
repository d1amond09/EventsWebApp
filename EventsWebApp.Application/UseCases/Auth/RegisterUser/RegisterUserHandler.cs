using AutoMapper;
using EventsWebApp.Application.DTOs;
using EventsWebApp.Domain.Contracts.Persistence;
using EventsWebApp.Domain.Entities;
using EventsWebApp.Domain.Responses;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace EventsWebApp.Application.UseCases.Auth.RegisterUser;

public class RegisterUserHandler(IRepositoryManager rep, IMapper mapper) : IRequestHandler<RegisterUserUseCase, ApiBaseResponse>
{
	private readonly IRepositoryManager _rep = rep;
	private readonly IMapper _mapper = mapper;

	public async Task<ApiBaseResponse> Handle(RegisterUserUseCase request, CancellationToken cancellationToken)
	{
		var user = _mapper.Map<User>(request.UserForRegistrationDto);

		var identityResult = await _rep.Users.RegisterAsync(user, request.UserForRegistrationDto.Password);

		if (identityResult.Succeeded)
		{
			if(request.UserForRegistrationDto.Roles?.Count > 0)
			{
				await _rep.Users
					.AddRolesToUserAsync(user, request.UserForRegistrationDto.Roles);
			}
			else
			{
				await _rep.Users
					.AddRolesToUserAsync(user, ["User"]);
			}

		}

		await _rep.SaveAsync();

		(IdentityResult, User) result = new(identityResult, user);

		return new ApiOkResponse<(IdentityResult, User)>(result);
	}
}