using EventsWebApp.Domain.Entities;
using EventsWebApp.Domain.Responses;
using MediatR;

namespace EventsWebApp.Application.UseCases.Auth.CreateToken;

public sealed record CreateTokenUseCase(User User, bool PopulateExp) : 
	IRequest<ApiBaseResponse>;