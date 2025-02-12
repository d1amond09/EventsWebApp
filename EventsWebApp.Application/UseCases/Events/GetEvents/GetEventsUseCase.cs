using EventsWebApp.Domain.RequestFeatures.ModelParameters;
using EventsWebApp.Domain.Responses;
using MediatR;

namespace EventsWebApp.Application.UseCases.Events.GetEvents;

public sealed record GetEventsUseCase(EventParameters EventParameters, bool TrackChanges) :
	IRequest<ApiBaseResponse>;
