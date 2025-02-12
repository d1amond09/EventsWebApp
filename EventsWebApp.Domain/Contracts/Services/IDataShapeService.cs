using EventsWebApp.Domain.Models;

namespace EventsWebApp.Domain.Contracts.Services;

public interface IDataShapeService<T>
{
	IEnumerable<ShapedEntity> ShapeData(IEnumerable<T> entities, string fieldsString);
	ShapedEntity ShapeData(T entity, string fieldsString);
}
