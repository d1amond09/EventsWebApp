using EventsWebApp.Domain.Entities;
using EventsWebApp.Domain.RequestFeatures.ModelParameters;
using EventsWebApp.Domain.RequestFeatures;

namespace EventsWebApp.Domain.Contracts;

public interface IParticipantRepository : IRepository<Participant>
{
	Task<PagedList<Participant>> GetWithPaginationAsync(ParticipantParameters parameters, bool trackChanges = false);
}
