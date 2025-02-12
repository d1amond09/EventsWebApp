using EventsWebApp.Application.DTOs;
using EventsWebApp.Domain.Responses;
using MediatR;

namespace EventsWebApp.Application.UseCases.Events.CreateEvent;

public sealed record CreateEventUseCase(EventForCreationDto Event) :
	IRequest<ApiBaseResponse>;
