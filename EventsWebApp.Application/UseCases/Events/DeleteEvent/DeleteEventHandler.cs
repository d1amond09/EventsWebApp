using AutoMapper;
using EventsWebApp.Application.DTOs;
using EventsWebApp.Domain.Contracts.Persistence;
using EventsWebApp.Domain.Entities;
using EventsWebApp.Domain.Responses;
using MediatR;

namespace EventsWebApp.Application.UseCases.Events.DeleteEvent;

public class DeleteEventHandler(IRepositoryManager rep) : IRequestHandler<DeleteEventUseCase, ApiBaseResponse>
{
	private readonly IRepositoryManager _rep = rep;

	public async Task<ApiBaseResponse> Handle(DeleteEventUseCase request, CancellationToken cancellationToken)
	{
		var evntToDelete = await _rep.Events.GetByIdAsync(request.Id, request.TrackChanges);

		if (evntToDelete is null)
			return new ApiNotFoundResponse("Event", request.Id);

		_rep.Events.Delete(evntToDelete);
		await _rep.SaveAsync();

		return new ApiOkResponse<Event>(evntToDelete);
	}
}