using EventsWebApp.Domain.Responses;
using MediatR;

namespace EventsWebApp.Application.UseCases.Participants.GetParticipantForEvent;

public sealed record GetParticipantForEventUseCase(Guid EventId, Guid Id, bool TrackChanges) :
	IRequest<ApiBaseResponse>;
