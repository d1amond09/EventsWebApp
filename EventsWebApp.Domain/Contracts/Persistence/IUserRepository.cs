using EventsWebApp.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace EventsWebApp.Domain.Contracts.Persistence;

public interface IUserRepository
{
	Task<User?> GetByIdAsync(Guid id, bool trackChanges = false);

	Task<User?> GetByEmailAsync(string email);

	Task<IdentityResult> RegisterAsync(User user, string password);

	Task<IdentityResult> UpdateAsync(User user);

	Task<IdentityResult> DeleteAsync(User user);

}
