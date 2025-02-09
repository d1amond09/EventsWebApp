using AutoMapper;
using EventsWebApp.Application.DTOs;
using EventsWebApp.Domain.Contracts.Persistence;
using EventsWebApp.Domain.Contracts.Services;
using EventsWebApp.Domain.Entities;
using EventsWebApp.Domain.Models;
using EventsWebApp.Domain.RequestFeatures;
using EventsWebApp.Domain.Responses;
using MediatR;

namespace EventsWebApp.Application.UseCases.Events.UpdateEvent;

public class UpdateEventHandler(IRepositoryManager rep, IMapper mapper) : IRequestHandler<UpdateEventUseCase, ApiBaseResponse>
{
	private readonly IRepositoryManager _rep = rep;
	private readonly IMapper _mapper = mapper;

	public async Task<ApiBaseResponse> Handle(UpdateEventUseCase request, CancellationToken cancellationToken)
	{
		var evntToUpdate = await _rep.Events.GetByIdAsync(request.Id, request.TrackChanges);

		if (evntToUpdate is null)
			return new ApiNotFoundResponse("Event", request.Id);

		_mapper.Map(request.Event, evntToUpdate);
		_rep.Events.Update(evntToUpdate);
		await _rep.SaveAsync();

		return new ApiOkResponse<Event>(evntToUpdate);
	}
}