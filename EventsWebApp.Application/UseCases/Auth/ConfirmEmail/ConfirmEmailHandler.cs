using EventsWebApp.Domain.Contracts.Persistence;
using EventsWebApp.Domain.Responses;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace EventsWebApp.Application.UseCases.Auth.ConfirmEmail;

public class ConfirmEmailHandler(IRepositoryManager rep) : IRequestHandler<ConfirmEmailUseCase, ApiBaseResponse>
{
	private readonly IRepositoryManager _rep = rep;

	public async Task<ApiBaseResponse> Handle(ConfirmEmailUseCase request, CancellationToken cancellationToken)
	{
		var user = await _rep.Users.GetByEmailAsync(request.Email) ?? throw new Exception();
		var result = await _rep.Users.ConfirmEmailAsync(user, request.Token);

		return new ApiOkResponse<IdentityResult>(result);
	}
}
