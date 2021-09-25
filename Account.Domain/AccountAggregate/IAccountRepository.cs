using System.Collections.Generic;
using System.Threading.Tasks;

namespace Account.Domain
{
    public interface IAccountRepository
    {
        Task InsertAsync(Account entity);

        Task UpdateAsync(Account entity);

        Task<Account> SelectAsync(string accountId);

        Task<Account> FindByEmailAsync(string customerEmail);

        Task<List<Account>> SelectAllAsync();

    }
}