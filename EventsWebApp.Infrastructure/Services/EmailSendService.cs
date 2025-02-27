﻿using EventsWebApp.Domain.ConfigurationModels;
using EventsWebApp.Domain.Contracts.Services;
using EventsWebApp.Domain.Exceptions;
using EventsWebApp.Domain.Models;
using MailKit.Net.Smtp;
using MimeKit;

namespace EventsWebApp.Infrastructure.Services;

public class EmailSendService(EmailConfiguration emailConfig) : IEmailSendService
{
	private readonly EmailConfiguration _emailConfig = emailConfig;

	public void SendEmail(Message message)
	{
		var emailMessage = CreateEmailMessage(message);

		Send(emailMessage);
	}

	private MimeMessage CreateEmailMessage(Message message)
	{
		var emailMessage = new MimeMessage();
		emailMessage.From.Add(new MailboxAddress("email", _emailConfig.From));
		emailMessage.To.AddRange(message.To);
		emailMessage.Subject = message.Subject;
		var bodyBuilder = new BodyBuilder { HtmlBody = string.Format("<p style='color:red;'>{0}</p>", message.Content) };
		if (message.Attachments != null && message.Attachments.Any())
		{
			byte[] fileBytes;
			foreach (var attachment in message.Attachments)
			{
				using (var ms = new MemoryStream())
				{
					attachment.CopyTo(ms);
					fileBytes = ms.ToArray();
				}
				bodyBuilder.Attachments.Add(attachment.FileName, fileBytes, ContentType.Parse(attachment.ContentType));
			}
		}
		emailMessage.Body = bodyBuilder.ToMessageBody();
		return emailMessage;
	}

	private void Send(MimeMessage mailMessage)
	{
		using var client = new SmtpClient();
		try
		{
			client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, true);
			client.AuthenticationMechanisms.Remove("XOAUTH2");
			client.Authenticate(_emailConfig.UserName, _emailConfig.Password);

			client.Send(mailMessage);
		}
		catch
		{
			throw new SendEmaiMessageException();
		}
		finally
		{
			client.Disconnect(true);
			client.Dispose();
		}
	}

	public async Task SendEmailAsync(Message message)
	{
		var mailMessage = CreateEmailMessage(message);
		await SendAsync(mailMessage);
	}

	private async Task SendAsync(MimeMessage mailMessage)
	{
		using var client = new SmtpClient();
		try
		{
			await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, true);
			client.AuthenticationMechanisms.Remove("XOAUTH2");
			await client.AuthenticateAsync(_emailConfig.UserName, _emailConfig.Password);

			await client.SendAsync(mailMessage);
		}
		catch
		{
			throw new SendEmaiMessageException();
		}
		finally
		{
			await client.DisconnectAsync(true);
			client.Dispose();
		}
	}
}
