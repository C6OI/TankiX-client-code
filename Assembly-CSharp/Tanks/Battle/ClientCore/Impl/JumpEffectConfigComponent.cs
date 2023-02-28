using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [Shared]
    [SerialVersionUID(1538548472363L)]
    public class JumpEffectConfigComponent : Component {
        public float ForceUpgradeMult { get; set; }
    }
}