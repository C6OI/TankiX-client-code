using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientHUD.Impl {
    [Shared]
    [SerialVersionUID(1512395506558L)]
    public class StreakTerminationEvent : Event {
        public string VictimUid { get; set; }
    }
}