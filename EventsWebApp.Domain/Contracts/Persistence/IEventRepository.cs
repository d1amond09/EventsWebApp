using EventsWebApp.Domain.Entities;
using EventsWebApp.Domain.RequestFeatures;
using EventsWebApp.Domain.RequestFeatures.ModelParameters;

namespace EventsWebApp.Domain.Contracts.Persistence;

public interface IEventRepository : IRepository<Event>
{
	Task<PagedList<Event>> GetWithPaginationAsync(EventParameters parameters, bool trackChanges = false);
	Task<PagedList<Event>> GetByUserWithPaginationAsync(EventParameters eventParameters, Guid userId, bool trackChanges);
}
