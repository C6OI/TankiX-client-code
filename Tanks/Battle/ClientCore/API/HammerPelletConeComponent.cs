using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.API {
    [Shared]
    [SerialVersionUID(1464955716416L)]
    public class HammerPelletConeComponent : Component {
        public float HorizontalConeHalfAngle { get; set; }

        public float VerticalConeHalfAngle { get; set; }

        public int PelletCount { get; set; }
    }
}