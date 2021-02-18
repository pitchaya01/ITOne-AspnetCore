using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITOne_AspnetCore.Application.Command;
using Lazarus.Common.ExceptionHandling;
using Lazarus.Common.Model;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ITOne_AspnetCore.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiException]

    public class CustomerController : ControllerBase
    {
        public IMediator _mediator;
        public CustomerController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<ResponseResult<bool>> Save(SaveCustomerCommand param)
        {
            //Test
            await _mediator.Send(param);
            return ResponseResult<bool>.Success();
        }
    }
}
