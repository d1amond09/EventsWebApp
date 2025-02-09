﻿using System.Text.Json;

namespace EventsWebApp.Domain.ErrorModel;

public class ErrorDetails
{
	public int StatusCode { get; set; }
	public string? Message { get; set; }
	public override string ToString() => JsonSerializer.Serialize(this);
}
