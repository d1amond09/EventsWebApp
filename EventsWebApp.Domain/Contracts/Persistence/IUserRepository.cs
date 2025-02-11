using EventsWebApp.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace EventsWebApp.Domain.Contracts.Persistence;

public interface IUserRepository
{
	Task<User?> GetByIdAsync(Guid id, bool trackChanges = false);
	Task<User?> GetByEmailAsync(string email);
	Task<User?> GetByUserNameAsync(string name);
	Task<IList<string>> GetRolesAsync(User user);

	Task<IdentityResult> RegisterAsync(User user, string password);
	Task<IdentityResult> AddRolesToUserAsync(User user, ICollection<string> roles);
	Task<IdentityResult> UpdateAsync(User user);
	Task<IdentityResult> DeleteAsync(User user);

	Task<IdentityResult> ConfirmEmailAsync(User user, string token);
	Task<bool> CheckPasswordAsync(User user, string password);
	Task<string> GenerateEmailConfirmationTokenAsync(User user);

}
