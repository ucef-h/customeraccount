using System;

namespace Core
{
    public interface IEntityRepository<TEntity, TId>
        where TEntity : IEntity<TId>
    {
    }

    public class EntityRepository<TEntity, TId> where TEntity : IEntity<TId>
    {
        private string _entityName;

        public virtual string EntityName
        {
            get => !string.IsNullOrEmpty(_entityName) ? _entityName : typeof(TEntity).Name;
            set => _entityName = value;
        }

        protected virtual void PreInsertEntity(TEntity entity)
        {
            entity.CreatedDate = entity.UpdatedDate = DateTime.UtcNow;
        }

        protected virtual void PreUpdateEntity(TEntity entity)
        {
            entity.UpdatedDate = DateTime.UtcNow;
        }
    }
}