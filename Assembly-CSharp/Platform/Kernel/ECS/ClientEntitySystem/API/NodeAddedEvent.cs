using Platform.Library.ClientProtocol.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.API {
    [SerialVersionUID(4343317511298323469L)]
    public class NodeAddedEvent : Event {
        public static readonly NodeAddedEvent Instance = new();
    }
}