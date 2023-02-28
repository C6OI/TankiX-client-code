using Platform.Library.ClientProtocol.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.API {
    [SerialVersionUID(2223729871283165639L)]
    public class NodeRemoveEvent : Event {
        public static readonly NodeRemoveEvent Instance = new();
    }
}