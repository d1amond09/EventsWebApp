using EventsWebApp.Domain.Entities;
using EventsWebApp.Domain.Responses;
using MediatR;

namespace EventsWebApp.Application.UseCases.Auth.SendEmailConfirmationToken;

public sealed record SendEmailConfirmationTokenUseCase(string ConfirmationLink, string Email) :
	IRequest<ApiBaseResponse>;