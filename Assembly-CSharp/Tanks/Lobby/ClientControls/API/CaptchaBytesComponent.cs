using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientControls.API {
    public class CaptchaBytesComponent : Component {
        public byte[] bytes;

        public CaptchaBytesComponent() { }

        public CaptchaBytesComponent(byte[] bytes) => this.bytes = bytes;
    }
}