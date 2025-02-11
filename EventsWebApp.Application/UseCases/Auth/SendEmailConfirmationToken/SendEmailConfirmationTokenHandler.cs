using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using EventsWebApp.Application.DTOs;
using EventsWebApp.Application.UseCases.Auth.RegisterUser;
using EventsWebApp.Application.UseCases.Events.CreateEvent;
using EventsWebApp.Domain.ConfigurationModels;
using EventsWebApp.Domain.Contracts.Persistence;
using EventsWebApp.Domain.Contracts.Services;
using EventsWebApp.Domain.Entities;
using EventsWebApp.Domain.Models;
using EventsWebApp.Domain.Responses;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Ocsp;

namespace EventsWebApp.Application.UseCases.Auth.SendEmailConfirmationToken;

public class SendEmailConfirmationTokenHandler(IRepositoryManager rep, IEmailSendService emailSender) :
	IRequestHandler<SendEmailConfirmationTokenUseCase, ApiBaseResponse>
{
	private readonly IRepositoryManager _rep = rep;
	private readonly IEmailSendService _emailSender = emailSender;

	public async Task<ApiBaseResponse> Handle(SendEmailConfirmationTokenUseCase request, CancellationToken cancellationToken)
	{
		var message = new Message([request.Email!], "Confirmation email link", request.ConfirmationLink!, null);
		await _emailSender.SendEmailAsync(message);

		return new ApiOkResponse<Message>(message);
	}
}
