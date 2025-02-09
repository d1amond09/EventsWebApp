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
	public async Task<PagedList<Participant>> GetWithPaginationAsync(ParticipantParameters participantParameters, bool trackChanges = false)
	{
		var participants = FindAll(trackChanges)
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

	public async Task<Participant?> GetByIdAsync(Guid id, bool trackChanges = false) =>
		await FindByCondition(c => c.Id.Equals(id), trackChanges)
			 .SingleOrDefaultAsync();

	public async Task<IEnumerable<Participant>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges = false) =>
		await FindByCondition(x => ids.Contains(x.Id), trackChanges)
			 .ToListAsync();
}
