using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.API {
    [Shared]
    [SerialVersionUID(1430205112111L)]
    public class GoldScheduledNotificationEvent : Event {
        public string Sender { get; set; }
    }
}