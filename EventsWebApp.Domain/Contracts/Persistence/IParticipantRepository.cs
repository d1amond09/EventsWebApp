using EventsWebApp.Domain.Entities;
using EventsWebApp.Domain.RequestFeatures;
using EventsWebApp.Domain.RequestFeatures.ModelParameters;

namespace EventsWebApp.Domain.Contracts.Persistence;

public interface IParticipantRepository : IRepository<Participant>
{
	Task<PagedList<Participant>> GetForEventWithPaginationAsync(Guid eventId, ParticipantParameters parameters, bool trackChanges = false);
	Task<int> GetCountOfParticipantsForEventAsync(Guid eventId, bool trackChanges = false);
	Task<bool> ParticipantContainsForEventAsync(Guid eventId, Guid userId, bool trackChanges = false);
	Task<Participant?> GetByIdForEventAsync(Guid eventId, Guid id, bool trackChanges = false);
	Task<Participant?> GetByUserIdForEventAsync(Guid eventId, Guid userId, bool trackChanges = false);
}
