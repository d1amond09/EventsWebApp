using AutoMapper;
using EventsWebApp.Domain.Contracts.Persistence;
using EventsWebApp.Domain.Responses;
using MediatR;

namespace EventsWebApp.Application.UseCases.Participants.CheckSubscribingToEvent;

public class CheckSubscribingToEventHandler(IRepositoryManager rep, IMapper mapper) : IRequestHandler<CheckSubscribingToEventUseCase, ApiBaseResponse>
{
	private readonly IRepositoryManager _rep = rep;

	public async Task<ApiBaseResponse> Handle(CheckSubscribingToEventUseCase request, CancellationToken cancellationToken)
	{
		bool participantSubscribed = await _rep.Participants.ParticipantContainsForEventAsync(request.EventId, request.UserId);

		return new ApiOkResponse<bool>(participantSubscribed);
	}
}