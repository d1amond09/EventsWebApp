using EventsWebApp.Application.DTOs;
using EventsWebApp.Domain.Responses;
using MediatR;

namespace EventsWebApp.Application.UseCases.Events.UpdateEvent;

public sealed record UpdateEventUseCase(Guid Id, EventForUpdateDto Event, bool TrackChanges) :
	IRequest<ApiBaseResponse>;
