using System;

namespace Lazarus.Common.CQRS.Command
{
    public interface ICommand
    {
        string AggregateId { get; set; }
    }
}
