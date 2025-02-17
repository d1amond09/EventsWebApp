using System.Security.Claims;
using System.Text.Json;
using EventsWebApp.API.Extensions;
using EventsWebApp.Application.DTOs;
using EventsWebApp.Application.UseCases.Participants.CheckSubscribingToEvent;
using EventsWebApp.Application.UseCases.Participants.GetParticipantForEvent;
using EventsWebApp.Application.UseCases.Participants.GetParticipantsForEvent;
using EventsWebApp.Application.UseCases.Participants.SubscribeToEvent;
using EventsWebApp.Application.UseCases.Participants.UnsubscribeFromEvent;
using EventsWebApp.Domain.Models;
using EventsWebApp.Domain.RequestFeatures.ModelParameters;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventsWebApp.API.Controllers;

[Consumes("application/json")]
[Route("api/events/{eventId}/participants")]
[ApiController]
public class ParticipantsController(ISender sender) : ControllerBase
{
	private readonly ISender _sender = sender;

	[HttpGet("{id:guid}", Name = "GetParticipantForEvent")]
	public async Task<IActionResult> GetParticipantForEvent(Guid eventId, Guid id)
	{
		var baseResult = await _sender.Send(new GetParticipantForEventUseCase(eventId, id, TrackChanges: false));

		var participant = baseResult.GetResult<ParticipantDto>();

		return Ok(participant);
	}

	[HttpGet(Name = "GetParticipantsForEvent")]
	public async Task<IActionResult> GetParticipantsForEvent(Guid eventId, [FromQuery] ParticipantParameters participantParameters)
	{
		var baseResult = await _sender.Send(new GetParticipantsForEventUseCase(eventId, participantParameters, TrackChanges: false));

		var response = baseResult.GetResult<ShapedEntitiesResponse>();

		Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(response.MetaData));

		return Ok(response.ShapedEntities);
	}

	[Authorize]
	[HttpPost(Name = "Subscribe")]
	public async Task<IActionResult> Subscribe(Guid eventId)
	{
		if (Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid userId))
		{
			var baseResult = await _sender.Send(new SubscribeToEventUseCase(eventId, userId));

			var result = baseResult.GetResult<string>();

			return Ok(result);
		}
		return BadRequest();
	}

	[Authorize]
	[HttpGet("check", Name = "CheckSubscribing")]
	public async Task<IActionResult> CheckSubscribing(Guid eventId)
	{
		if (Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid userId))
		{
			var baseResult = await _sender.Send(new CheckSubscribingToEventUseCase(eventId, userId));

			var result = baseResult.GetResult<bool>();

			return Ok(result);
		}
		return BadRequest();
	}

	[Authorize]
	[HttpDelete(Name = "Unsubscribe")]
	public async Task<IActionResult> Unsubscribe(Guid eventId)
	{
		if (Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid userId))
		{
			var baseResult = await _sender.Send(new UnsubscribeFromEventUseCase(eventId, userId, TrackChanges: false));
			var result = baseResult.GetResult<string>();
			return Ok(result);
		}
		return BadRequest();
	}
}
