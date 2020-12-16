using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITOne_AspnetCore.Application.Command;
using Lazarus.Common.Model;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ITOne_AspnetCore.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]

    public class CustomerController : ControllerBase
    {
        public IMediator _mediator;
        public CustomerController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public ResponseResult<bool> Save(SaveCustomerCommand param)
        {
            _mediator.Send(param);
            return ResponseResult<bool>.Success();
        }
    }
}
