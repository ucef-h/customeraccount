using Account.Domain;
using Core;
using MediatR;
using MongoDB.Bson;
using System.Threading.Tasks;

namespace Account.Infrastructure
{
    public class AccountDbContext : IUnitOfWork<Domain.Account>
    {
        private readonly IMediator _mediator;
        private readonly IAccountRepository _accountRepository;

        public AccountDbContext(
            IAccountRepository accountRepository,
            IMediator mediator)
        {
            _accountRepository = accountRepository;
            _mediator = mediator;
        }

        public async Task<bool> SaveEntitiesAsync(Domain.Account entity)
        {
            var isTransient = await CheckIsTransient(entity);

            await _mediator.DispatchDomainEventsAsync(entity);

            if (isTransient)
            {
                await _accountRepository.InsertAsync(entity);
            }
            else
            {
                await _accountRepository.UpdateAsync(entity);
            }

            return true;
        }

        private async Task<bool> CheckIsTransient(Domain.Account entity)
        {
            bool isTransient = string.IsNullOrEmpty(entity.Id);
            if (isTransient) entity.Id = ObjectId.GenerateNewId().ToString();
            else isTransient = await _accountRepository.SelectAsync(entity.Id) == null;
            return isTransient;
        }
    }
}