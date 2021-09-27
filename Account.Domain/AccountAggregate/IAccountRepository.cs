using System.Collections.Generic;
using System.Threading.Tasks;

namespace Account.Domain
{
    public interface IAccountRepository
    {
        Task InsertAsync(Account entity);

        Task UpdateAsync(Account entity);
        Task<Account> SelectAsync(string accountId);
        Task<List<Account>> SelectAllAsync();
        Task<Account> GetByEmail(string accountEmail);
    }
}