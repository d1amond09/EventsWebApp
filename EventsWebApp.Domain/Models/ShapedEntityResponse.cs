using EventsWebApp.Domain.RequestFeatures;

namespace EventsWebApp.Domain.Models;

public class ShapedEntitiesResponse(IEnumerable<ShapedEntity> shapedEntities, MetaData metaData)
{
	public IEnumerable<Entity> ShapedEntities { get; set; } = shapedEntities.Select(s => s.Entity);
	public MetaData MetaData { get; set; } = metaData;
}
