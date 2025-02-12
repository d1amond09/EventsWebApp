using EventsWebApp.Application.DTOs;
using EventsWebApp.Domain.Responses;
using MediatR;

namespace EventsWebApp.Application.UseCases.Auth.LoginUser;

public sealed record LoginUserUseCase(UserForAuthenticationDto UserToLogin) :
	IRequest<ApiBaseResponse>;