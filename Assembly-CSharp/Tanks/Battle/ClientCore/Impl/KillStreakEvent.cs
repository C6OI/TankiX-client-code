using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [Shared]
    [SerialVersionUID(1491556721814L)]
    public class KillStreakEvent : Event {
        public int Score { get; set; }
    }
}