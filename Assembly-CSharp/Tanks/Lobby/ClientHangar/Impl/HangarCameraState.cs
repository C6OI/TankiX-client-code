using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientHangar.Impl {
    public class HangarCameraState {
        public class Enabled : Node {
            public HangarCameraEnabledComponent hangarCameraEnabled;
        }

        public class Disabled : Node {
            public HangarCameraDisabledComponent hangarCameraDisabled;
        }
    }
}