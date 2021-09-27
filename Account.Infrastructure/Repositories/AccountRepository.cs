using Account.Domain;
using Account.Domain.Exceptions;
using Core;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Account.Infrastructure.Repositories
{
    public class AccountRepository : EntityRepository<Domain.Account, string>, IAccountRepository
    {
        private readonly IMongoCollection<Domain.Account> _account;

        public AccountRepository(IMongoClient client)
        {
            _account = client.GetDatabase("customer-account").GetCollection<Domain.Account>(EntityName);
        }

        public sealed override string EntityName => "account";

        public async Task InsertAsync(Domain.Account entity)
        {
            if (string.IsNullOrEmpty(entity.Id))
            {
                entity.Id = ObjectId.GenerateNewId().ToString();
            }

            PreInsertEntity(entity);
            await _account.InsertOneAsync(entity);
        }

        public async Task UpdateAsync(Domain.Account entity)
        {
            PreUpdateEntity(entity);
            await _account.ReplaceOneAsync(filter => filter.Id.Equals(entity.Id), entity);
        }

        public async Task<Domain.Account> SelectAsync(string accountId)
        {
            var account = await _account.Find(e => e.Id.Equals(accountId)).FirstOrDefaultAsync();
            if (account == null)
            {
                throw new AccountNotFoundException(accountId, nameof(Domain.Account.Id));
            }

            account.ReApplyAll();
            return account;
        }

        public async Task<List<Domain.Account>> SelectAllAsync()
        {
            var account = await _account.Find(e => true).ToListAsync();

            account?.ForEach(e => e.ReApplyAll());
            return account;
        }

        public async Task<Domain.Account> GetByEmail(string accountEmail)
        {
            var account = await _account.Find(e => e.Email.Equals(accountEmail)).FirstOrDefaultAsync();
            if (account == null)
            {
                throw new AccountNotFoundException(accountEmail, nameof(Domain.Account.Email));
            }

            account.ReApplyAll();
            return account;
        }
    }
}