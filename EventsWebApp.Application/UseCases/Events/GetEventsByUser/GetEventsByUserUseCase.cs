using EventsWebApp.Domain.RequestFeatures.ModelParameters;
using EventsWebApp.Domain.Responses;
using MediatR;

namespace EventsWebApp.Application.UseCases.Events.GetEventsByUser;

public sealed record GetEventsByUserUseCase(Guid UserId, EventParameters EventParameters, bool TrackChanges) :
	IRequest<ApiBaseResponse>;
