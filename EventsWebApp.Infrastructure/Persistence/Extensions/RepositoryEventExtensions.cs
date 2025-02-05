using EventsWebApp.Infrastructure.Persistence.Extensions.Utility;
using EventsWebApp.Domain.Entities;
using System.Linq.Dynamic.Core;

namespace EventsWebApp.Infrastructure.Persistence.Extensions;

public static class RepositoryEventExtensions
{
	public static IQueryable<Event> FilterByDateTime(this IQueryable<Event> events, DateTime minDateTime, DateTime maxDateTime) =>
		events.Where(e => e.DateTime >= minDateTime && e.DateTime <= maxDateTime);

	public static IQueryable<Event> SearchByCategory(this IQueryable<Event> events, string category)
	{
		if (string.IsNullOrWhiteSpace(category))
			return events;

		return events.Where(e => e.Category.Contains(category, StringComparison.CurrentCultureIgnoreCase));
	}

	public static IQueryable<Event> SearchByLocation(this IQueryable<Event> events, string location)
	{
		if (string.IsNullOrWhiteSpace(location))
			return events;

		return events.Where(e => e.Location.Contains(location, StringComparison.CurrentCultureIgnoreCase));
	}

	public static IQueryable<Event> SearchByName(this IQueryable<Event> events, string name)
	{
		if (string.IsNullOrWhiteSpace(name))
			return events;

		return events.Where(e => e.Name.Contains(name, StringComparison.CurrentCultureIgnoreCase));
	}

	public static IQueryable<Event> Sort(this IQueryable<Event> events, string orderByQueryString)
	{
		if (string.IsNullOrWhiteSpace(orderByQueryString))
			return events.OrderBy(e => e.Name);

		var orderQuery = OrderQueryBuilder.CreateOrderQuery<Event>(orderByQueryString);

		if (string.IsNullOrWhiteSpace(orderQuery))
			return events.OrderBy(e => e.Name);

		return events.OrderBy(orderQuery);
	}
}
