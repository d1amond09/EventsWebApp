using EventsWebApp.Application.DTOs;
using EventsWebApp.Domain.Responses;
using MediatR;

namespace EventsWebApp.Application.UseCases.Auth.RefreshToken;

public sealed record RefreshTokenUseCase(TokenDto TokenDto) :
	IRequest<ApiBaseResponse>;