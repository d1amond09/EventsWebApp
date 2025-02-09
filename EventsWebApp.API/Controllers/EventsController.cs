using EventsWebApp.Domain.RequestFeatures;
using System.Text.Json;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EventsWebApp.Domain.RequestFeatures.ModelParameters;
using EventsWebApp.Application.UseCases.Events.GetEvents;
using EventsWebApp.API.Extensions;
using EventsWebApp.Domain.Models;
using EventsWebApp.Application.UseCases.Events.GetEvent;
using EventsWebApp.Application.DTOs;
using EventsWebApp.Application.UseCases.Events.CreateEvent;
using EventsWebApp.Application.UseCases.Events.DeleteEvent;
using EventsWebApp.Application.UseCases.Events.UpdateEvent;

namespace EventsWebApp.API.Controllers;

[Consumes("application/json")]
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
	public async Task<IActionResult> GetCourse(Guid id)
	{
		var baseResult = await _sender.Send(new GetEventUseCase(id, TrackChanges: false));

		var products = baseResult.GetResult<EventDto>();

		return Ok(products);
	}

	[HttpPost(Name = "CreateEvent")]
	public async Task<IActionResult> CreateCourse([FromBody] EventForCreationDto evnt)
	{
		var baseResult = await _sender.Send(new CreateEventUseCase(evnt));

		var createdProduct = baseResult.GetResult<EventDto>();

		return CreatedAtRoute("GetEvent", new { id = createdProduct.Id }, createdProduct);
	}

	[HttpDelete("{id:guid}")]
	public async Task<IActionResult> DeleteCourse(Guid id)
	{
		var baseResult = await _sender.Send(new DeleteEventUseCase(id, TrackChanges: false));

		return NoContent();
	}

	[HttpPut("{id:guid}")]
	public async Task<IActionResult> UpdateCourse(Guid id, [FromBody] EventForUpdateDto evnt)
	{
		var baseResult = await _sender.Send(new UpdateEventUseCase(id, evnt, TrackChanges: true));

		return NoContent();
	}
}
