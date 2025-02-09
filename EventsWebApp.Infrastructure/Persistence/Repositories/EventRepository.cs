using EventsWebApp.Domain.Contracts.Persistence;
using EventsWebApp.Domain.Entities;
using EventsWebApp.Domain.RequestFeatures;
using EventsWebApp.Domain.RequestFeatures.ModelParameters;
using EventsWebApp.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace EventsWebApp.Infrastructure.Persistence.Repositories;

public class EventRepository(AppDbContext appDbContext) :
	RepositoryBase<Event>(appDbContext), IEventRepository
{
	public async Task<PagedList<Event>> GetWithPaginationAsync(EventParameters eventParameters, bool trackChanges)
	{
		var events = FindAll(trackChanges)
			.FilterByDateTime(eventParameters.MinDateTime, eventParameters.MaxDateTime)
			.SearchByLocation(eventParameters.Location)
			.SearchByCategory(eventParameters.Category)
			.SearchByName(eventParameters.Name);

		var count = await events.CountAsync();

		var eventsToList = await events
			.Sort(eventParameters.OrderBy)
			.Skip((eventParameters.PageNumber - 1) * eventParameters.PageSize)
			.Take(eventParameters.PageSize)
			.ToListAsync();

		return new PagedList<Event>(
			eventsToList,
			count,
			eventParameters.PageNumber,
			eventParameters.PageSize
		);
	}

	public async Task<Event?> GetByIdAsync(Guid id, bool trackChanges) =>
		await FindByCondition(c => c.Id.Equals(id), trackChanges)
			 .SingleOrDefaultAsync();

	public async Task<IEnumerable<Event>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges = false) =>
		await FindByCondition(x => ids.Contains(x.Id), trackChanges)
			 .ToListAsync();
}
