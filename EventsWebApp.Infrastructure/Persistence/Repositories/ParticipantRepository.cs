using EventsWebApp.Domain.Contracts.Persistence;
using EventsWebApp.Domain.Entities;
using EventsWebApp.Domain.RequestFeatures;
using EventsWebApp.Domain.RequestFeatures.ModelParameters;
using EventsWebApp.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace EventsWebApp.Infrastructure.Persistence.Repositories;

public class ParticipantRepository(AppDbContext appDbContext) :
	RepositoryBase<Participant>(appDbContext), IParticipantRepository
{
	public async Task<PagedList<Participant>> GetForEventWithPaginationAsync(Guid eventId, ParticipantParameters participantParameters, bool trackChanges = false)
	{
		var participants = FindAll(trackChanges).Include(p => p.User)
			.Where(p => p.EventId.Equals(eventId))
			.FilterByRegisteredAt(participantParameters.MinRegisteredAt, participantParameters.MaxRegisteredAt);

		var count = await participants.CountAsync();

		var participantsToList = await participants
			.Sort(participantParameters.OrderBy)
			.Skip((participantParameters.PageNumber - 1) * participantParameters.PageSize)
			.Take(participantParameters.PageSize)
			.ToListAsync();

		return new PagedList<Participant>(
			participantsToList,
			count,
			participantParameters.PageNumber,
			participantParameters.PageSize
		);
	}

	public async Task<bool> ParticipantContainsForEventAsync(Guid eventId, Guid userId, bool trackChanges = false)
	{
		var participants = FindAll(trackChanges)
			.Where(p => p.EventId.Equals(eventId) && p.UserId.Equals(userId));

		var count = await participants.CountAsync();

		return count > 0;
	}

	public async Task<int> GetCountOfParticipantsForEventAsync(Guid eventId, bool trackChanges = false)
	{
		var participants = FindAll(trackChanges)
			.Where(p => p.EventId.Equals(eventId));

		var count = await participants.CountAsync();

		return count;
	}

	public async Task<Participant?> GetByUserIdForEventAsync(Guid eventId, Guid userId, bool trackChanges = false) =>
		await FindByCondition(p => p.UserId.Equals(userId) && p.EventId.Equals(eventId), trackChanges).Include(p => p.User)
			 .SingleOrDefaultAsync();

	public async Task<Participant?> GetByIdForEventAsync(Guid eventId, Guid id, bool trackChanges = false) =>
		await FindByCondition(p => p.Id.Equals(id) && p.EventId.Equals(eventId), trackChanges).Include(p => p.User)
			 .SingleOrDefaultAsync();

	public async Task<Participant?> GetByIdAsync(Guid id, bool trackChanges = false) =>
		await FindByCondition(p => p.Id.Equals(id), trackChanges).Include(p => p.User)
			 .SingleOrDefaultAsync();

	public async Task<IEnumerable<Participant>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges = false) =>
		await FindByCondition(p => ids.Contains(p.Id), trackChanges).Include(p => p.User)
			 .ToListAsync();
}
