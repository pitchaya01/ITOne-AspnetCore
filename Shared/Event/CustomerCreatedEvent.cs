using Lazarus.Common.EventMessaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Event
{
    public class CustomerCreatedEvent: IntegrationEvent
    {
        public string Name { get; set; }
    }
}
