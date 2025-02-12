using EventsWebApp.Domain.RequestFeatures.ModelParameters;
using EventsWebApp.Domain.Responses;
using MediatR;

namespace EventsWebApp.Application.UseCases.Participants.UnsubscribeFromEvent;

public sealed record UnsubscribeFromEventUseCase(Guid EventId, Guid UserId, bool TrackChanges) :
	IRequest<ApiBaseResponse>;
