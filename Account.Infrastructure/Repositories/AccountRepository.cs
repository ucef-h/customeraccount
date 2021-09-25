﻿using Account.Domain;
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
        private readonly IMongoCollection<Domain.Account> _application;

        public AccountRepository(IMongoClient client)
        {
            _application = client.GetDatabase("customer-account").GetCollection<Domain.Account>(EntityName);
        }

        public sealed override string EntityName => "account";

        public async Task InsertAsync(Domain.Account entity)
        {
            if (string.IsNullOrEmpty(entity.Id))
            {
                entity.Id = ObjectId.GenerateNewId().ToString();
            }

            PreInsertEntity(entity);
            await _application.InsertOneAsync(entity);
        }

        public async Task UpdateAsync(Domain.Account entity)
        {
            PreUpdateEntity(entity);
            await _application.ReplaceOneAsync(filter => filter.Id.Equals(entity.Id), entity);
        }

        public async Task<Domain.Account> SelectAsync(string accountId)
        {
            var account = await _application.Find(e => e.Id.Equals(accountId)).FirstOrDefaultAsync();
            if (account == null)
            {
                throw new AccountNotFoundException(accountId);
            }

            account.ReApplyAll();
            return account;
        }

        public async Task<List<Domain.Account>> SelectAllAsync()
        {
            return await _application.Find(e => true).ToListAsync();
        }
    }
}