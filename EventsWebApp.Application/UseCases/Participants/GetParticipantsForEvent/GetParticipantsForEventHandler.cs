using AutoMapper;
using EventsWebApp.Application.DTOs;
using EventsWebApp.Domain.Contracts.Persistence;
using EventsWebApp.Domain.Contracts.Services;
using EventsWebApp.Domain.Models;
using EventsWebApp.Domain.Responses;
using MediatR;

namespace EventsWebApp.Application.UseCases.Participants.GetParticipantsForEvent;

public class GetParticipantsForEventHandler(IDataShapeService<ParticipantDto> dataShaper, IRepositoryManager rep, IMapper mapper) :
	IRequestHandler<GetParticipantsForEventUseCase, ApiBaseResponse>
{
	private readonly IDataShapeService<ParticipantDto> _dataShaper = dataShaper;
	private readonly IRepositoryManager _rep = rep;
	private readonly IMapper _mapper = mapper;

	public async Task<ApiBaseResponse> Handle(GetParticipantsForEventUseCase request, CancellationToken cancellationToken)
	{
		var evnt = await _rep.Events.GetByIdAsync(request.EventId, request.TrackChanges);

		if (evnt is null)
			return new ApiNotFoundResponse("Event", request.EventId);

		if (request.ParticipantParameters.NotValidRegisteredAtRange)
			return new ApiNotValidRangeBadRequestResponse("RegisteredAt");

		var participantsWithMetaData = await _rep.Participants.GetForEventWithPaginationAsync(
			request.EventId,
			request.ParticipantParameters,
			request.TrackChanges
		);

		var participantsDto = _mapper.Map<IEnumerable<ParticipantDto>>(participantsWithMetaData);
		var shapedParticipants = _dataShaper.ShapeData(participantsDto, request.ParticipantParameters.Fields);

		ShapedEntitiesResponse result = new(shapedParticipants, participantsWithMetaData.MetaData);

		return new ApiOkResponse<ShapedEntitiesResponse>(result);
	}
}