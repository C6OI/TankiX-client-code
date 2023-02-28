using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientCore.API {
    public class MouseControlStateHolderComponent : Component {
        public bool MouseControlAllowed { get; set; }

        public bool MouseVerticalInverted { get; set; }

        public bool MouseControlEnable { get; set; }

        public float MouseSensivity { get; set; }
    }
}