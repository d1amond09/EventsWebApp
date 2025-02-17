using AutoMapper;
using EventsWebApp.Domain.Contracts.Persistence;
using EventsWebApp.Domain.Responses;
using MediatR;

namespace EventsWebApp.Application.UseCases.Participants.UnsubscribeFromEvent;

public class UnsubscribeFromEventHandler(IRepositoryManager rep, IMapper mapper) : IRequestHandler<UnsubscribeFromEventUseCase, ApiBaseResponse>
{
	private readonly IRepositoryManager _rep = rep;

	public async Task<ApiBaseResponse> Handle(UnsubscribeFromEventUseCase request, CancellationToken cancellationToken)
	{
		var evnt = await _rep.Events.GetByIdAsync(request.EventId, request.TrackChanges);

		if (evnt is null)
			return new ApiNotFoundResponse("Event", request.EventId);

		var participant = await _rep.Participants.GetByUserIdForEventAsync(request.EventId, request.UserId, request.TrackChanges);

		if (participant is null)
			return new ApiNotFoundResponse("Participant", request.UserId);

		if (participant.UserId == request.UserId)
		{
			_rep.Participants.Delete(participant);
			await _rep.SaveAsync();
			return new ApiOkResponse<string>("You have successfully unsubscribed from the event.");
		}
		return new ApiBadRequestResponse("You are not subscribed to this event yet.");
	}
}