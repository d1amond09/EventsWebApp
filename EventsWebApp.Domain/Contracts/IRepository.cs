using EventsWebApp.Domain.Entities;
using EventsWebApp.Domain.RequestFeatures;

namespace EventsWebApp.Domain.Contracts;

public interface IRepository<T> where T : class
{
	Task<IEnumerable<T>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges = false);
	Task<T?> GetByIdAsync(Guid id, bool trackChanges = false);
	void Create(T evnt);
	void Delete(T evnt);
	Task SaveAsync();
}
