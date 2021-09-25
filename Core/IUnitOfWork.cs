using System.Threading.Tasks;

namespace Core
{
    public interface IUnitOfWork<TBaseEntity> where TBaseEntity : BaseEntity
    {
        Task<bool> SaveEntitiesAsync(TBaseEntity entity);
    }
}
