using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(636338223435643707L)]
    public class TargetFocusVerticalTargetingComponent : Component {
        public float AdditionalAngleUp { get; set; }

        public float AdditionalAngleDown { get; set; }
    }
}