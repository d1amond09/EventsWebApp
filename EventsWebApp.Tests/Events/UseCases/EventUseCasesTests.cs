using AutoMapper;
using EventsWebApp.Application.DTOs;
using EventsWebApp.Application.UseCases.Events.CreateEvent;
using EventsWebApp.Application.UseCases.Events.DeleteEvent;
using EventsWebApp.Application.UseCases.Events.GetEvent;
using EventsWebApp.Application.UseCases.Events.GetEvents;
using EventsWebApp.Application.UseCases.Events.GetEventsByUser;
using EventsWebApp.Application.UseCases.Events.UpdateEvent;
using EventsWebApp.Domain.Contracts.Persistence;
using EventsWebApp.Domain.Contracts.Services;
using EventsWebApp.Domain.Entities;
using EventsWebApp.Domain.Models;
using EventsWebApp.Domain.RequestFeatures;
using EventsWebApp.Domain.RequestFeatures.ModelParameters;
using EventsWebApp.Domain.Responses;
using Moq;

namespace EventsWebApp.Tests.Events.UseCases;

public class UseCasesTests
{
	private readonly Mock<IRepositoryManager> _repositoryManagerMock;
	private readonly Mock<IMapper> _mapperMock;
	private readonly Mock<IDataShapeService<EventDto>> _dataShapeServiceMock;

	public UseCasesTests()
	{
		_repositoryManagerMock = new Mock<IRepositoryManager>();
		_mapperMock = new Mock<IMapper>();
		_dataShapeServiceMock = new Mock<IDataShapeService<EventDto>>();
	}

	[Fact]
	public async Task GetEventsHandler_ReturnsCorrectResponse()
	{
		// Arrange
		var eventParameters = new EventParameters { };
		var trackChanges = false;
		var getEventsUseCase = new GetEventsUseCase(eventParameters, trackChanges);
		var eventsWithMetaData = new PagedList<Event>(new List<Event>(), 1, 10, 20);
		var eventsDto = new List<EventDto> { new EventDto() };
		var shapedEvents = new List<ShapedEntity> { new ShapedEntity() };
		var shapedEntitiesResponse = new ShapedEntitiesResponse(shapedEvents, eventsWithMetaData.MetaData);

		_repositoryManagerMock.Setup(r => r.Events.GetWithPaginationAsync(eventParameters, trackChanges))
			.ReturnsAsync(eventsWithMetaData);
		_mapperMock.Setup(m => m.Map<IEnumerable<EventDto>>(eventsWithMetaData))
			.Returns(eventsDto);
		_dataShapeServiceMock.Setup(d => d.ShapeData(eventsDto, eventParameters.Fields))
			.Returns(shapedEvents);

		var handler = new GetEventsHandler(_repositoryManagerMock.Object, _mapperMock.Object, _dataShapeServiceMock.Object);

		// Act
		var response = await handler.Handle(getEventsUseCase, CancellationToken.None);

		// Assert
		Assert.IsType<ApiOkResponse<ShapedEntitiesResponse>>(response);
		Assert.Equivalent(shapedEntitiesResponse, (response as ApiOkResponse<ShapedEntitiesResponse>)?.Result);
	}

	[Fact]
	public async Task GetEventHandler_ReturnsCorrectResponse()
	{
		// Arrange
		var trackChanges = false;
		var getEventUseCase = new GetEventUseCase(Guid.NewGuid(), trackChanges);
		var event1 = new Event();
		var eventDto = new EventDto();

		_repositoryManagerMock.Setup(r => r.Events.GetByIdAsync(getEventUseCase.Id, trackChanges))
			.ReturnsAsync(event1);
		_mapperMock.Setup(m => m.Map<EventDto>(event1))
			.Returns(eventDto);

		var handler = new GetEventHandler(_repositoryManagerMock.Object, _mapperMock.Object);

		// Act
		var response = await handler.Handle(getEventUseCase, CancellationToken.None);

		// Assert
		Assert.IsType<ApiOkResponse<EventDto>>(response);
		Assert.Equal(eventDto, (response as ApiOkResponse<EventDto>)?.Result);
	}

	[Fact]
	public async Task DeleteEventHandler_ReturnsCorrectResponse()
	{
		// Arrange
		var trackChanges = false;
		var deleteEventUseCase = new DeleteEventUseCase(Guid.NewGuid(), trackChanges);
		var event1 = new Event();

		_repositoryManagerMock.Setup(r => r.Events.GetByIdAsync(deleteEventUseCase.Id, trackChanges))
			.ReturnsAsync(event1);
		_repositoryManagerMock.Setup(r => r.Events.Delete(event1));
		_repositoryManagerMock.Setup(r => r.SaveAsync());

		var handler = new DeleteEventHandler(_repositoryManagerMock.Object);

		// Act
		var response = await handler.Handle(deleteEventUseCase, CancellationToken.None);

		// Assert
		Assert.IsType<ApiOkResponse<Event>>(response);
		Assert.Equal(event1, (response as ApiOkResponse<Event>)?.Result);
	}


	[Fact]
	public async Task CreateEventHandler_ReturnsCorrectResponse()
	{
		// Arrange
		var createEventUseCase = new CreateEventUseCase(new EventForCreationDto());
		var newEvent = new Event();
		var createdEventDto = new EventDto();

		_mapperMock.Setup(m => m.Map<Event>(createEventUseCase.Event))
			.Returns(newEvent);
		_repositoryManagerMock.Setup(r => r.Events.Create(newEvent));
		_repositoryManagerMock.Setup(r => r.SaveChanges());
		_mapperMock.Setup(m => m.Map<EventDto>(newEvent))
			.Returns(createdEventDto);

		var handler = new CreateEventHandler(_repositoryManagerMock.Object, _mapperMock.Object);

		// Act
		var response = await handler.Handle(createEventUseCase, CancellationToken.None);

		// Assert
		Assert.IsType<ApiOkResponse<EventDto>>(response);
		Assert.Equal(createdEventDto, (response as ApiOkResponse<EventDto>)?.Result);
	}

	[Fact]
	public async Task UpdateEventHandler_ReturnsCorrectResponse()
	{
		// Arrange
		var trackChanges = true;
		var updateEventUseCase = new UpdateEventUseCase(Guid.NewGuid(), new EventForUpdateDto(), true);
		var existingEvent = new Event() {
			DateTime = new DateTime(2025, 2, 12, 14, 0, 36, 956)
		};
		var updatedEvent = new Event() {
			DateTime = new DateTime(2025, 2, 12, 14, 0, 36, 956)
		};

		_repositoryManagerMock.Setup(r => r.Events.GetByIdAsync(updateEventUseCase.Id, trackChanges))
			.ReturnsAsync(existingEvent);
		_mapperMock.Setup(m => m.Map(updateEventUseCase.Event, existingEvent))
			.Returns(updatedEvent);
		_repositoryManagerMock.Setup(r => r.Events.Update(updatedEvent));
		_repositoryManagerMock.Setup(r => r.SaveChanges());

		var handler = new UpdateEventHandler(_repositoryManagerMock.Object, _mapperMock.Object);

		// Act
		var response = await handler.Handle(updateEventUseCase, CancellationToken.None);

		// Assert
		Assert.IsType<ApiOkResponse<Event>>(response);
		Assert.Equivalent(updatedEvent, (response as ApiOkResponse<Event>)?.Result);
	}

	[Fact]
	public async Task GetEventsByUserHandler_ReturnsCorrectResponse()
	{
		// Arrange
		var trackChanges = false;
		var getUserEventsUseCase = new GetEventsByUserUseCase(Guid.NewGuid(), new EventParameters(), trackChanges);
		var eventsWithMetaData = new PagedList<Event>([new()], 1, 10, 20);
		var eventsDto = new List<EventDto> { new() };
		var shapedEvents = new List<ShapedEntity> { new() };
		var shapedEntitiesResponse = new ShapedEntitiesResponse(shapedEvents, eventsWithMetaData.MetaData);

		_repositoryManagerMock.Setup(r => r.Events.GetByUserWithPaginationAsync(getUserEventsUseCase.EventParameters, getUserEventsUseCase.UserId, trackChanges))
			.ReturnsAsync(eventsWithMetaData);
		_mapperMock.Setup(m => m.Map<IEnumerable<EventDto>>(eventsWithMetaData))
			.Returns(eventsDto);
		_dataShapeServiceMock.Setup(d => d.ShapeData(eventsDto, getUserEventsUseCase.EventParameters.Fields))
			.Returns(shapedEvents);

		var handler = new GetEventsByUserHandler(_repositoryManagerMock.Object, _mapperMock.Object, _dataShapeServiceMock.Object);

		// Act
		var response = await handler.Handle(getUserEventsUseCase, CancellationToken.None);

		// Assert
		Assert.IsType<ApiOkResponse<ShapedEntitiesResponse>>(response);
		Assert.Equivalent(shapedEntitiesResponse, (response as ApiOkResponse<ShapedEntitiesResponse>)?.Result);
	}
}