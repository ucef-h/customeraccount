using Account.Application;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Account.Api.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ResponseBase<AccountResponse>> Get(AccountInfoRequest accountInfo)
        {
            var result = await _mediator.Send(new AccountQuery(accountInfo.Id));
            return new ResponseBase<AccountResponse>(result, true);
        }

        [HttpPost]
        public async Task<ResponseBase<string>> Withdraw(AccountAmountRequest accountInfo)
        {
            var result = await _mediator.Send(new WithdrawAccountCommand(accountInfo.Id, accountInfo.Amount));
            return new ResponseBase<string>(result);
        }

        [HttpPost]
        public async Task<ResponseBase<string>> Deposit(AccountAmountRequest accountInfo)
        {
            var result = await _mediator.Send(new DepositAccountCommand(accountInfo.Id, accountInfo.Amount));
            return new ResponseBase<string>(result);
        }
    }
}