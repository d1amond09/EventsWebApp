namespace EventsWebApp.Domain.Contracts.Persistence;

public interface IRepositoryManager
{
	IEventRepository Events { get; }
	IParticipantRepository Participants { get; }
	IUserRepository Users { get; }
	Task SaveAsync();
	void SaveChanges();
}
