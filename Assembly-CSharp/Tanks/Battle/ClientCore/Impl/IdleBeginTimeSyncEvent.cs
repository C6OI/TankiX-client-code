using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Platform.Library.ClientUnityIntegration.API;

namespace Tanks.Battle.ClientCore.Impl {
    [Shared]
    [SerialVersionUID(4633772578502170850L)]
    public class IdleBeginTimeSyncEvent : Event {
        public Date IdleBeginTime { get; set; }
    }
}