using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.System.Data.Exchange.ClientNetwork.API {
    public class ServerConnectionCloseReasonEvent : Event {
        public string Reason { get; set; }
    }
}