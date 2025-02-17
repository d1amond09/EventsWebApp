using EventsWebApp.Domain.Entities;
using EventsWebApp.Domain.RequestFeatures.ModelParameters;
using EventsWebApp.Infrastructure.Persistence;
using EventsWebApp.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EventsWebApp.Tests.Events.Repository
{
	public class EventRepositoryTests
	{
		private Guid _userId;
		private List<Event> _events;
		private readonly DbContextOptions<AppDbContext> _options;

		public EventRepositoryTests()
		{
			_options = new DbContextOptionsBuilder<AppDbContext>()
				.UseInMemoryDatabase(databaseName: "EventsDb")
				.Options;

			using var context = new AppDbContext(_options);
			_userId = Guid.NewGuid();

			_events =
			[
				new() {
					Id = Guid.NewGuid(),
					Name = "Test Event 1",
					Description = "This is a test event 1",
					Category = "Test Category",
					Location = "Test Location",
					DateTime = DateTime.Now.AddDays(1),
				},
				new() {
					Id = Guid.NewGuid(),
					Name = "Test Event 2",
					Description = "This is a test event 2",
					Category = "Test Category",
					Location = "Test Location",
					DateTime = DateTime.Now.AddDays(2),
				},
				new(){
					Id = Guid.NewGuid(),
					Name = "Test Event 3",
					Description = "This is a test event 3",
					Category = "Test Category",
					Location = "Test Location",
					DateTime = DateTime.Now.AddDays(3),
					MaxCountParticipants = 1,
					Image = null
				},
				new() {
					Id = Guid.NewGuid(),
					Name = "Test Event 4",
					Description = "This is a test event 4",
					Category = "Test Category",
					Location = "Test Location",
					DateTime = DateTime.Now.AddDays(4),
					Participants =
					[
						new() {
							UserId = _userId
						}
					]
				},
				new() {
					Id = Guid.NewGuid(),
					Name = "Test Event 5",
					Description = "This is a test event 5",
					Category = "Test Category",
					Location = "Test Location",
					DateTime = DateTime.Now.AddDays(5),
					Participants =[
						new() {
							UserId = Guid.NewGuid()
						}
					]
				}

			];
		}

		[Fact]
		public async Task GetByIdAsync_ShouldReturnEvent_WhenEventExists()
		{
			// Arrange
			using var context = new AppDbContext(_options);
			context.Events.AddRange(_events);
			context.SaveChanges();
			var repository = new EventRepository(context);
			var evnt = context.Events.First();

			// Act
			var result = await repository.GetByIdAsync(evnt.Id, true);
			context.Events.RemoveRange(_events);
			context.SaveChanges();

			// Assert
			Assert.NotNull(result);
			Assert.Equal(evnt.Id, result.Id);
		}

		[Fact]
		public async Task GetWithPaginationAsync_ShouldReturnPagedList_WhenEventExists()
		{
			// Arrange
			using var context = new AppDbContext(_options);
			context.Events.AddRange(_events);
			context.SaveChanges();
			var repository = new EventRepository(context);
			var parameters = new EventParameters
			{
				PageNumber = 1,
				PageSize = 10,
				MinDateTime = DateTime.Now.AddDays(1),
				MaxDateTime = DateTime.Now.AddDays(4)
			};

			// Act
			var result = await repository.GetWithPaginationAsync(parameters, true);
			context.Events.RemoveRange(_events);
			context.SaveChanges();

			// Assert
			Assert.NotNull(result);
			Assert.Equal(3, result.Count);
		}

		[Fact]
		public async Task GetByUserWithPaginationAsync_ShouldReturnPagedList_WhenEventExists()
		{
			// Arrange
			using var context = new AppDbContext(_options);
			context.Events.AddRange(_events);
			context.SaveChanges();
			var repository = new EventRepository(context);
			var parameters = new EventParameters
			{
				PageNumber = 1,
				PageSize = 10,
				MinDateTime = DateTime.Now.AddDays(-1),
				MaxDateTime = DateTime.Now.AddDays(9)
			};

			// Act
			var result = await repository.GetByUserWithPaginationAsync(parameters, _userId, true);
			context.Events.RemoveRange(_events);
			context.SaveChanges();

			// Assert
			Assert.NotNull(result);
			Assert.Equal(1, result.Count);
		}
	}
}
