using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientHangar.Impl {
    public class HangarCameraRotationState {
        public class Enabled : Node {
            public HangarCameraRotationEnabledComponent hangarCameraRotationEnabled;
        }

        public class Disabled : Node {
            public HangarCameraRotationDisabledComponent hangarCameraRotationDisabled;
        }
    }
}