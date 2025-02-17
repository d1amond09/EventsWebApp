using System.Linq.Dynamic.Core;
using EventsWebApp.Domain.Entities;
using EventsWebApp.Infrastructure.Persistence.Extensions.Utility;

namespace EventsWebApp.Infrastructure.Persistence.Extensions;

public static class RepositoryParticipantExtensions
{
	public static IQueryable<Participant> FilterByRegisteredAt(this IQueryable<Participant> participants, DateTime minDateTime, DateTime maxDateTime) =>
		participants.Where(e => e.RegisteredAt >= minDateTime && e.RegisteredAt <= maxDateTime);

	public static IQueryable<Participant> Sort(this IQueryable<Participant> participants, string orderByQueryString)
	{
		if (string.IsNullOrWhiteSpace(orderByQueryString))
			return participants.OrderBy(e => e.RegisteredAt);

		var orderQuery = OrderQueryBuilder.CreateOrderQuery<Event>(orderByQueryString);

		if (string.IsNullOrWhiteSpace(orderQuery))
			return participants.OrderBy(e => e.RegisteredAt);

		return participants.OrderBy(orderQuery);
	}
}
