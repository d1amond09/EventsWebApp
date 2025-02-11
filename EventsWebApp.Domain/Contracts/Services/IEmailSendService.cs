using EventsWebApp.Domain.Models;

namespace EventsWebApp.Domain.Contracts.Services;

public interface IEmailSendService
{
	void SendEmail(Message message);
	Task SendEmailAsync(Message message);
}
