using EventsWebApp.Domain.Contracts.Services;
using EventsWebApp.Domain.Models;
using EventsWebApp.Domain.Responses;
using MediatR;

namespace EventsWebApp.Application.UseCases.Auth.SendEmailConfirmationToken;

public class SendEmailConfirmationTokenHandler(IEmailSendService emailSender) :
	IRequestHandler<SendEmailConfirmationTokenUseCase, ApiBaseResponse>
{
	private readonly IEmailSendService _emailSender = emailSender;

	public async Task<ApiBaseResponse> Handle(SendEmailConfirmationTokenUseCase request, CancellationToken cancellationToken)
	{
		var message = new Message([request.Email!], "Confirmation email link", request.ConfirmationLink!, null);
		await _emailSender.SendEmailAsync(message);

		return new ApiOkResponse<Message>(message);
	}
}
