using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(636338223011879469L)]
    public class TargetFocusConicTargetingComponent : Component {
        public float AdditionalHalfConeAngle { get; set; }
    }
}