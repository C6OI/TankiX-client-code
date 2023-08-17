using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.API {
    [Shared]
    [SerialVersionUID(-8835994525014820133L)]
    public class KillEvent : Event {
        public Entity Target { get; set; }
    }
}