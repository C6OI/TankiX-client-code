using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(636340664888926847L)]
    public class TargetFocusPelletConeComponent : Component {
        public bool ChangePelletCount { get; set; }

        public float AdditionalHorizontalHalfConeAngle { get; set; }

        public float AdditionalVerticalHalfConeAngle { get; set; }
    }
}