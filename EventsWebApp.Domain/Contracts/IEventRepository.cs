using EventsWebApp.Domain.Entities;
using EventsWebApp.Domain.RequestFeatures.ModelParameters;
using EventsWebApp.Domain.RequestFeatures;

namespace EventsWebApp.Domain.Contracts;

public interface IEventRepository : IRepository<Event>
{
	Task<PagedList<Event>> GetWithPaginationAsync(EventParameters parameters, bool trackChanges = false);
}
