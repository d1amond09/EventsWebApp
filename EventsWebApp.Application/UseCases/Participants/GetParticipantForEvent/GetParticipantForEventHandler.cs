using AutoMapper;
using EventsWebApp.Application.DTOs;
using EventsWebApp.Domain.Contracts.Persistence;
using EventsWebApp.Domain.Responses;
using MediatR;

namespace EventsWebApp.Application.UseCases.Participants.GetParticipantForEvent;

public class GetParticipantForEventHandler(IRepositoryManager rep, IMapper mapper) : IRequestHandler<GetParticipantForEventUseCase, ApiBaseResponse>
{
	private readonly IRepositoryManager _rep = rep;
	private readonly IMapper _mapper = mapper;

	public async Task<ApiBaseResponse> Handle(GetParticipantForEventUseCase request, CancellationToken cancellationToken)
	{
		var evnt = await _rep.Events.GetByIdAsync(request.EventId, request.TrackChanges);

		if (evnt is null)
			return new ApiNotFoundResponse("Event", request.EventId);

		var participant = await _rep.Participants.GetByIdForEventAsync(request.EventId, request.Id, request.TrackChanges);

		if (participant is null)
			return new ApiNotFoundResponse("Participant", request.Id);

		var participantDto = _mapper.Map<ParticipantDto>(participant);
		return new ApiOkResponse<ParticipantDto>(participantDto);
	}
}