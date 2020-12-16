using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazarus.Common.Domain.Seedwork
{
    public interface IDomainEvent : INotification
    {
        DateTime OccurredOn { get; }
    }
}
