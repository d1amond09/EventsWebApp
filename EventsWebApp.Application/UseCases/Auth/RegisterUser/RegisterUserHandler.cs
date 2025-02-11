using System;
using AutoMapper;
using EventsWebApp.Application.DTOs;
using EventsWebApp.Application.UseCases.Events.CreateEvent;
using EventsWebApp.Domain.Contracts.Persistence;
using EventsWebApp.Domain.Entities;
using EventsWebApp.Domain.Models;
using EventsWebApp.Domain.Responses;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Org.BouncyCastle.Asn1.Ocsp;

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
			await _rep.Users
				.AddRolesToUserAsync(user, request.UserForRegistrationDto.Roles);

		await _rep.SaveAsync();

		(IdentityResult, User) result = new(identityResult, user);

		return new ApiOkResponse<(IdentityResult, User)>(result);
	}
}