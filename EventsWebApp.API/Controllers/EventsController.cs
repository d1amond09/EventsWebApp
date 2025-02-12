using EventsWebApp.Application.UseCases.Events.CreateEvent;
using EventsWebApp.Application.UseCases.Events.DeleteEvent;
using EventsWebApp.Application.UseCases.Events.UpdateEvent;
using EventsWebApp.Domain.RequestFeatures.ModelParameters;
using EventsWebApp.Application.UseCases.Events.GetEvents;
using EventsWebApp.Application.UseCases.Events.GetEvent;
using EventsWebApp.Application.DTOs;
using EventsWebApp.API.Extensions;
using EventsWebApp.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace EventsWebApp.API.Controllers;

[Route("api/events")]
[ApiController]
public class EventsController(ISender sender) : ControllerBase
{
	private readonly ISender _sender = sender;

	[HttpGet(Name = "GetEvents")]
	public async Task<IActionResult> GetEvents([FromQuery] EventParameters eventParameters)
	{
		var baseResult = await _sender.Send(new GetEventsUseCase(eventParameters, TrackChanges: false));

		var response = baseResult.GetResult<ShapedEntitiesResponse>();

		Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(response.MetaData));

		return Ok(response.ShapedEntities);
	}

	[HttpGet("{id:guid}", Name = "GetEvent")]
	public async Task<IActionResult> GetEvent(Guid id)
	{
		var baseResult = await _sender.Send(new GetEventUseCase(id, TrackChanges: false));

		var evnt = baseResult.GetResult<EventDto>();

		return Ok(evnt);
	}

	[Authorize]
	[HttpGet("me", Name = "GetEventsByUser")]
	public async Task<IActionResult> GetEventsByUser([FromQuery] EventParameters eventParameters)
	{
		if (Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid userId))
		{
			var baseResult = await _sender.Send(new GetEventsUseCase(eventParameters, TrackChanges: false));

			var response = baseResult.GetResult<ShapedEntitiesResponse>();

			Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(response.MetaData));
			return Ok(response.ShapedEntities);
		}
		return BadRequest();
	}

	[Authorize(Roles ="Administrator")]
	[HttpPost(Name = "CreateEvent")]
	public async Task<IActionResult> CreateEvent([FromForm] EventForCreationDto evnt)
	{
		var baseResult = await _sender.Send(new CreateEventUseCase(evnt));

		var createdProduct = baseResult.GetResult<EventDto>();

		return CreatedAtRoute("GetEvent", new { id = createdProduct.Id }, createdProduct);
	}

	[Authorize(Roles = "Administrator")]
	[HttpDelete("{id:guid}", Name="DeleteEvent")]
	public async Task<IActionResult> DeleteEvent(Guid id)
	{
		var baseResult = await _sender.Send(new DeleteEventUseCase(id, TrackChanges: false));

		return NoContent();
	}

	[Authorize(Roles = "Administrator")]
	[HttpPut("{id:guid}", Name = "UpdateEvent")]
	public async Task<IActionResult> UpdateEvent(Guid id, [FromBody] EventForUpdateDto evnt)
	{
		var baseResult = await _sender.Send(new UpdateEventUseCase(id, evnt, TrackChanges: true));

		return NoContent();
	}
}
