using System.Threading.Tasks;

namespace Lazarus.Common.EventMessaging
{
    public interface IDynamicIntegrationEventHandler
    {
        Task Handle(dynamic eventData);
    }
}