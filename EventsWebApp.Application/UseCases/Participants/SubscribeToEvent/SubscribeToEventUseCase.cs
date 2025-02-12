using EventsWebApp.Domain.RequestFeatures.ModelParameters;
using EventsWebApp.Domain.Responses;
using MediatR;

namespace EventsWebApp.Application.UseCases.Participants.SubscribeToEvent;

public sealed record SubscribeToEventUseCase(Guid EventId, Guid UserId) :
	IRequest<ApiBaseResponse>;
