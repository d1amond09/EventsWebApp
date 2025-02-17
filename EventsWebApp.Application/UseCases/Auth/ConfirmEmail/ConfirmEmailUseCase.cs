using EventsWebApp.Domain.Responses;
using MediatR;

namespace EventsWebApp.Application.UseCases.Auth.ConfirmEmail;

public sealed record ConfirmEmailUseCase(string Email, string Token) :
	IRequest<ApiBaseResponse>;