using EventsWebApp.Domain.Responses;
using MediatR;

namespace EventsWebApp.Application.UseCases.Participants.CheckSubscribingToEvent;

public sealed record CheckSubscribingToEventUseCase(Guid EventId, Guid UserId) :
	IRequest<ApiBaseResponse>;
