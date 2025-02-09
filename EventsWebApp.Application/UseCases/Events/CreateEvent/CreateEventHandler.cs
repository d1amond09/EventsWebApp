using AutoMapper;
using EventsWebApp.Application.DTOs;
using EventsWebApp.Domain.Contracts.Persistence;
using EventsWebApp.Domain.Entities;
using EventsWebApp.Domain.Responses;
using MediatR;

namespace EventsWebApp.Application.UseCases.Events.CreateEvent;

public class CreateEventHandler(IRepositoryManager rep, IMapper mapper) : IRequestHandler<CreateEventUseCase, ApiBaseResponse>
{
	private readonly IRepositoryManager _rep = rep;
	private readonly IMapper _mapper = mapper;

	public async Task<ApiBaseResponse> Handle(CreateEventUseCase request, CancellationToken cancellationToken)
	{
		var entityToCreate = _mapper.Map<Event>(request.Event);

		_rep.Events.Create(entityToCreate);
		await _rep.SaveAsync();

		var entityToReturn = _mapper.Map<EventDto>(entityToCreate);
		return new ApiOkResponse<EventDto>(entityToReturn);
	}
}