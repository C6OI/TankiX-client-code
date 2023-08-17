using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientHangar.Impl {
    public class HangarCameraDecelerationRotateComponent : Component {
        public float Speed { get; set; }

        public int LastUpdateFrame { get; set; }
    }
}