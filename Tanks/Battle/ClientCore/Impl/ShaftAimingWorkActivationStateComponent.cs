using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [Shared]
    [SerialVersionUID(8631717637564140236L)]
    public class ShaftAimingWorkActivationStateComponent : TimeValidateComponent {
        public ShaftAimingWorkActivationStateComponent() => ActivationTimer = 0f;

        public float ActivationTimer { get; set; }
    }
}