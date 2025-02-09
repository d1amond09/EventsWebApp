using EventsWebApp.Application.DTOs;
using EventsWebApp.Domain.Responses;
using MediatR;

namespace EventsWebApp.Application.UseCases.Events.DeleteEvent;

public sealed record DeleteEventUseCase(Guid Id, bool TrackChanges) :
	IRequest<ApiBaseResponse>;
