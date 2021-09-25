using System;

namespace Core
{
    public interface IEntity<TId>
    {
        TId Id { get; set; }
        DateTime CreatedDate { get; set; }
        DateTime UpdatedDate { get; set; }
    }

    public abstract class Entity<TId> : IEntity<TId>
    {
        public virtual TId Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public string CreatedBy { get; set; }
        public string LastUpdatedBy { get; set; }
    }
}