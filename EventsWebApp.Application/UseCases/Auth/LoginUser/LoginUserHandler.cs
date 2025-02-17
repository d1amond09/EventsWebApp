using EventsWebApp.Domain.Contracts.Persistence;
using EventsWebApp.Domain.Entities;
using EventsWebApp.Domain.Responses;
using MediatR;

namespace EventsWebApp.Application.UseCases.Auth.LoginUser;

public class LoginUserHandler(IRepositoryManager rep) : IRequestHandler<LoginUserUseCase, ApiBaseResponse>
{
	private readonly IRepositoryManager _rep = rep;

	public async Task<ApiBaseResponse> Handle(LoginUserUseCase request, CancellationToken cancellationToken)
	{
		var user = await _rep.Users.GetByUserNameAsync(request.UserToLogin.UserName);

		bool isValid = user != null &&
			await _rep.Users.CheckPasswordAsync(user, request.UserToLogin.Password);

		(bool, User) result = new(isValid, user);

		return new ApiOkResponse<(bool, User?)>(result);
	}
}