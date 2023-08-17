using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Platform.Library.ClientUnityIntegration.API;

namespace Tanks.Battle.ClientCore.API {
    [SerialVersionUID(635824352644415226L)]
    public class LocalDurationComponent : Component {
        public LocalDurationComponent() { }

        public LocalDurationComponent(float duration) => Duration = duration;

        public Date StartedTime { get; set; }

        public float Duration { get; set; }

        public bool IsComplete { get; set; }

        public ScheduleManager LocalDurationExpireEvent { get; set; }
    }
}