using EventsWebApp.Domain.Contracts.Persistence;
using EventsWebApp.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace EventsWebApp.Infrastructure.Persistence.Repositories;

public class UserRepository(UserManager<User> userManager) : IUserRepository
{
	private readonly UserManager<User> _userManager = userManager;

	public async Task<User?> GetByIdAsync(Guid id, bool trackChanges = false) =>
		await _userManager.FindByIdAsync(id.ToString());
	public async Task<User?> GetByEmailAsync(string email) =>
		await _userManager.FindByEmailAsync(email);
	public async Task<User?> GetByUserNameAsync(string name) =>
		await _userManager.FindByNameAsync(name);
	public async Task<IList<string>> GetRolesAsync(User user) =>
		await _userManager.GetRolesAsync(user);

	public async Task<IdentityResult> AddRolesToUserAsync(User user, ICollection<string> roles) =>
		await _userManager.AddToRolesAsync(user, roles);
	public async Task<IdentityResult> RegisterAsync(User user, string password) =>
		await _userManager.CreateAsync(user, password);
	public async Task<IdentityResult> UpdateAsync(User user) =>
		await _userManager.UpdateAsync(user);
	public async Task<IdentityResult> DeleteAsync(User user) =>
		await _userManager.DeleteAsync(user);

	public async Task<IdentityResult> ConfirmEmailAsync(User user, string token) =>
		await _userManager.ConfirmEmailAsync(user, token);

	public async Task<bool> CheckPasswordAsync(User user, string password) =>
		await _userManager.CheckPasswordAsync(user, password);

	public async Task<string> GenerateEmailConfirmationTokenAsync(User user) =>
		await _userManager.GenerateEmailConfirmationTokenAsync(user);

}
