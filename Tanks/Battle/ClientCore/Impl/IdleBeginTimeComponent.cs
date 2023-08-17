using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class IdleBeginTimeComponent : SharedChangeableComponent {
        Date? idleBeginTime;

        public Date? IdleBeginTime {
            get => idleBeginTime;
            set {
                idleBeginTime = value;
                OnChange();
            }
        }
    }
}