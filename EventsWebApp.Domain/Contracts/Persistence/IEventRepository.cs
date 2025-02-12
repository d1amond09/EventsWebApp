using EventsWebApp.Domain.Entities;
using EventsWebApp.Domain.RequestFeatures.ModelParameters;
using EventsWebApp.Domain.RequestFeatures;

namespace EventsWebApp.Domain.Contracts.Persistence;

public interface IEventRepository : IRepository<Event>
{
	Task<PagedList<Event>> GetWithPaginationAsync(EventParameters parameters, bool trackChanges = false);
	Task<PagedList<Event>> GetByUserWithPaginationAsync(EventParameters eventParameters, Guid userId, bool trackChanges);
}
