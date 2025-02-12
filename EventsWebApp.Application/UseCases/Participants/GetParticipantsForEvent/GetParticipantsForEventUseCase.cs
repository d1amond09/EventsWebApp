using EventsWebApp.Domain.RequestFeatures.ModelParameters;
using EventsWebApp.Domain.Responses;
using MediatR;

namespace EventsWebApp.Application.UseCases.Participants.GetParticipantsForEvent;

public sealed record GetParticipantsForEventUseCase(Guid EventId, ParticipantParameters ParticipantParameters, bool TrackChanges) :
	IRequest<ApiBaseResponse>;
