using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(6541712051864507498L)]
    [Shared]
    public class ShaftWaitingStateComponent : TimeValidateComponent {
        public ShaftWaitingStateComponent() => WaitingTimer = 0f;

        public float WaitingTimer { get; set; }
    }
}