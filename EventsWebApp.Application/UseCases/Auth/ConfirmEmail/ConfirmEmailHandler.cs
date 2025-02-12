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

namespace EventsWebApp.Application.UseCases.Auth.ConfirmEmail;

public class ConfirmEmailHandler(IRepositoryManager rep) : IRequestHandler<ConfirmEmailUseCase, ApiBaseResponse>
{
	private readonly IRepositoryManager _rep = rep;

	public async Task<ApiBaseResponse> Handle(ConfirmEmailUseCase request, CancellationToken cancellationToken)
	{
		var user = await _rep.Users.GetByEmailAsync(request.Email) ?? throw new Exception();
		var result = await _rep.Users.ConfirmEmailAsync(user, request.Token);
		
		return new ApiOkResponse<IdentityResult>(result);
	}
}
