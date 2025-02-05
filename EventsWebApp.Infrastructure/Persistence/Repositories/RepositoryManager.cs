using EventsWebApp.Domain.Contracts;
using EventsWebApp.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EventsWebApp.Infrastructure.Persistence.Repositories;

public class RepositoryManager : IRepositoryManager
{
	public AppDbContext DbContext => _dbContext;
	private readonly AppDbContext _dbContext;
	private readonly UserManager<User> _userManager;
	private readonly Lazy<IEventRepository> _eventRep;
	private readonly Lazy<IParticipantRepository> _partRep;
	private readonly Lazy<IUserRepository> _userRep;
	public RepositoryManager(AppDbContext dbContext, UserManager<User> userManager)
	{
		_dbContext = dbContext;
		_userManager = userManager;
		_eventRep = new Lazy<IEventRepository>(() => new EventRepository(_dbContext));
		_partRep = new Lazy<IParticipantRepository>(() => new ParticipantRepository(_dbContext));
		_userRep = new Lazy<IUserRepository>(() => new UserRepository(_userManager));
	}

	public IEventRepository Events => _eventRep.Value;
	public IParticipantRepository Participants => _partRep.Value;
	public IUserRepository Users => _userRep.Value;
	public async Task SaveAsync() => await _dbContext.SaveChangesAsync();
	public void SaveChanges() => _dbContext.SaveChanges();
}
