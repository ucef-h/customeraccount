using Customer.Application;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Customer.Api.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class CustomerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CustomerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ResponseBase<bool>> Add(CustomerInfoRequest customerInfo)
        {
            var result = await _mediator.Send(new InsertCustomerCommand(
                customerInfo.Name, customerInfo.Email, customerInfo.Credits)
            );
            return new ResponseBase<bool>(result);
        }
    }
}