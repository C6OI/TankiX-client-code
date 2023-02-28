using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(636338224015916897L)]
    public class TargetFocusVerticalSectorTargetingComponent : Component {
        public float AdditionalAngleUp { get; set; }

        public float AdditionalAngleDown { get; set; }

        public float AdditionalAngleHorizontal { get; set; }
    }
}