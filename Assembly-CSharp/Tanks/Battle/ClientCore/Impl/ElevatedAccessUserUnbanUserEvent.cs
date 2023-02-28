using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [Shared]
    [SerialVersionUID(1512742576673L)]
    public class ElevatedAccessUserUnbanUserEvent : Event {
        public string Uid { get; set; }
    }
}