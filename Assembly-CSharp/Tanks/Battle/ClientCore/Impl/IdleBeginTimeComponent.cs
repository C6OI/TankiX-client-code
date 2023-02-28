using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class IdleBeginTimeComponent : Component {
        public Date? IdleBeginTime { get; set; }
    }
}