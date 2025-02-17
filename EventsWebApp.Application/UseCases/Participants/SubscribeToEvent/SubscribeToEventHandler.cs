using AutoMapper;
using EventsWebApp.Domain.Contracts.Persistence;
using EventsWebApp.Domain.Entities;
using EventsWebApp.Domain.Responses;
using MediatR;

namespace EventsWebApp.Application.UseCases.Participants.SubscribeToEvent;

public class SubscribeToEventHandler(IRepositoryManager rep, IMapper mapper) : IRequestHandler<SubscribeToEventUseCase, ApiBaseResponse>
{
	private readonly IRepositoryManager _rep = rep;

	public async Task<ApiBaseResponse> Handle(SubscribeToEventUseCase request, CancellationToken cancellationToken)
	{
		var evnt = await _rep.Events.GetByIdAsync(request.EventId, false);
		int count = await _rep.Participants.GetCountOfParticipantsForEventAsync(request.EventId);
		bool participantSubscribed = await _rep.Participants.ParticipantContainsForEventAsync(request.EventId, request.UserId);

		if (participantSubscribed)
		{
			return new ApiBadRequestResponse("You are already subscribed to this event.");
		}

		if (evnt?.MaxCountParticipants <= count)
		{
			return new ApiBadRequestResponse("This event has the maximum number of participants.");
		}

		var participantToCreate = new Participant
		{
			EventId = request.EventId,
			UserId = request.UserId,
		};

		_rep.Participants.Create(participantToCreate);
		await _rep.SaveAsync();

		return new ApiOkResponse<string>("You have successfully subscribed to the event.");
	}
}