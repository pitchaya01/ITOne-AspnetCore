using Lazarus.Common.EventMessaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazarus.Common.Domain.SharedEvent
{
    public class SendedEmailInviteEvent : IntegrationEvent
    {
        public Guid AggregateId { get; set; }
        public string Email { get; set; }
    }
}
