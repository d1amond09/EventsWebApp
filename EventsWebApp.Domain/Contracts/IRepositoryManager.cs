namespace EventsWebApp.Domain.Contracts;

public interface IRepositoryManager
{
	IEventRepository Events { get; }
	IParticipantRepository Participants { get; }
	IUserRepository Users { get; }
	Task SaveAsync();
	void SaveChanges();
}
