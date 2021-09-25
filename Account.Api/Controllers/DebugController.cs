using Account.Application;
using Account.Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Account.Api.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class DebuggerController : ControllerBase
    {
        private readonly IAccountRepository _repository;

        public DebuggerController(IWebHostEnvironment environment, IAccountRepository repository)
        {
            _repository = repository;
            if (!environment.IsDevelopment())
            {
                throw new Exception("Cannot Access Resource");
            }
        }

        [HttpGet]
        public async Task<ResponseBase<List<Domain.Account>>> GetAllAccounts()
        {
            return new ResponseBase<List<Domain.Account>>(await _repository.SelectAllAsync());
        }
    }
}