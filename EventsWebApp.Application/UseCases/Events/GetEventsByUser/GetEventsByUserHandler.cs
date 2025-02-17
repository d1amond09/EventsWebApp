using AutoMapper;
using EventsWebApp.Application.DTOs;
using EventsWebApp.Domain.Contracts.Persistence;
using EventsWebApp.Domain.Contracts.Services;
using EventsWebApp.Domain.Models;
using EventsWebApp.Domain.Responses;
using MediatR;

namespace EventsWebApp.Application.UseCases.Events.GetEventsByUser;

public class GetEventsByUserHandler(IRepositoryManager rep, IMapper mapper, IDataShapeService<EventDto> dataShaper) :
	IRequestHandler<GetEventsByUserUseCase, ApiBaseResponse>
{
	private readonly IRepositoryManager _rep = rep;
	private readonly IMapper _mapper = mapper;
	private readonly IDataShapeService<EventDto> _dataShaper = dataShaper;

	public async Task<ApiBaseResponse> Handle(GetEventsByUserUseCase request, CancellationToken cancellationToken)
	{
		if (request.EventParameters.NotValidDateTimeRange)
			return new ApiNotValidRangeBadRequestResponse("dateTime");

		var eventsWithMetaData = await _rep.Events.GetByUserWithPaginationAsync(
			request.EventParameters,
			request.UserId,
			request.TrackChanges
		);

		var eventsDto = _mapper.Map<IEnumerable<EventDto>>(eventsWithMetaData);
		var shapedEvents = _dataShaper.ShapeData(eventsDto, request.EventParameters.Fields);

		ShapedEntitiesResponse result = new(shapedEvents, eventsWithMetaData.MetaData);

		return new ApiOkResponse<ShapedEntitiesResponse>(result);
	}
}