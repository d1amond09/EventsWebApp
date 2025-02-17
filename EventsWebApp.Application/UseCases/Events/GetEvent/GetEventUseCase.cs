using EventsWebApp.Domain.Responses;
using MediatR;

namespace EventsWebApp.Application.UseCases.Events.GetEvent;

public sealed record GetEventUseCase(Guid Id, bool TrackChanges) :
	IRequest<ApiBaseResponse>;
