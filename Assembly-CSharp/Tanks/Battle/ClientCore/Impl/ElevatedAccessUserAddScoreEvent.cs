using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [Shared]
    [SerialVersionUID(1514369648926L)]
    public class ElevatedAccessUserAddScoreEvent : Event {
        public int Count { get; set; }
    }
}