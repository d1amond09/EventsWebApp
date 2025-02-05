using System.Linq;
using EventsWebApp.Domain.Contracts;
using EventsWebApp.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EventsWebApp.Infrastructure.Persistence.Repositories;

public class UserRepository(UserManager<User> userManager) : IUserRepository
{
	private readonly UserManager<User> _userManager = userManager;
	public async Task<User?> GetByIdAsync(Guid id, bool trackChanges = false) =>
		await _userManager.FindByIdAsync(id.ToString());

	public async Task<User?> GetByEmailAsync(string email)
	 => await _userManager.FindByEmailAsync(email);

	public async Task<IdentityResult> RegisterAsync(User user, string password) =>
		await _userManager.CreateAsync(user, password);

	public async Task<IdentityResult> UpdateAsync(User user) =>
		await _userManager.UpdateAsync(user);

	public async Task<IdentityResult> DeleteAsync(User user) => 
		await _userManager.DeleteAsync(user);

}
