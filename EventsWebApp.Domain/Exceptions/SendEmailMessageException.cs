namespace EventsWebApp.Domain.Exceptions;

public sealed class SendEmaiMessageException() :
	Exception("Error with SmtpClient during sending email message")
{
}
