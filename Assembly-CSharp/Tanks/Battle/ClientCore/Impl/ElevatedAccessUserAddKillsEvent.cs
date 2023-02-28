using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [Shared]
    [SerialVersionUID(1515485268330L)]
    public class ElevatedAccessUserAddKillsEvent : Event {
        public int Count { get; set; }
    }
}