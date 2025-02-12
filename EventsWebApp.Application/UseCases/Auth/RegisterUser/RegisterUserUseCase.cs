using EventsWebApp.Application.DTOs;
using EventsWebApp.Domain.Responses;
using MediatR;

namespace EventsWebApp.Application.UseCases.Auth.RegisterUser;

public sealed record RegisterUserUseCase(UserForRegistrationDto UserForRegistrationDto) :
	IRequest<ApiBaseResponse>;