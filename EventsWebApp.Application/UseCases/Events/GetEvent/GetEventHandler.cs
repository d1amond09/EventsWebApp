using AutoMapper;
using EventsWebApp.Application.DTOs;
using EventsWebApp.Domain.Contracts.Persistence;
using EventsWebApp.Domain.Responses;
using MediatR;

namespace EventsWebApp.Application.UseCases.Events.GetEvent;

public class GetEventHandler(IRepositoryManager rep, IMapper mapper) : IRequestHandler<GetEventUseCase, ApiBaseResponse>
{
	private readonly IRepositoryManager _rep = rep;
	private readonly IMapper _mapper = mapper;

	public async Task<ApiBaseResponse> Handle(GetEventUseCase request, CancellationToken cancellationToken)
	{
		var evnt = await _rep.Events.GetByIdAsync(request.Id, request.TrackChanges);

		if (evnt is null)
			return new ApiNotFoundResponse("Event", request.Id);

		var evntDto = _mapper.Map<EventDto>(evnt);
		return new ApiOkResponse<EventDto>(evntDto);
	}
}