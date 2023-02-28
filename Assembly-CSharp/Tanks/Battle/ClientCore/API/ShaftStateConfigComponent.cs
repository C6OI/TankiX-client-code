using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.API {
    [SerialVersionUID(635950079224407790L)]
    [Shared]
    public class ShaftStateConfigComponent : Component {
        public float WaitingToActivationTransitionTimeSec { get; set; }

        public float ActivationToWorkingTransitionTimeSec { get; set; }

        public float FinishToIdleTransitionTimeSec { get; set; }
    }
}