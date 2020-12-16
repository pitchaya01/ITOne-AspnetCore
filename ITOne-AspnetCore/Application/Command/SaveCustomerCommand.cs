using MediatR;
using System.Collections.Generic;
using System.Linq;

namespace ITOne_AspnetCore.Application.Command
{
    public class SaveCustomerCommand:IRequest
    {
        public string Name { get; set; }
    }
}
