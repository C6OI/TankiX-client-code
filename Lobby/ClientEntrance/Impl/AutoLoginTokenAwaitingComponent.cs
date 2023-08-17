using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Lobby.ClientEntrance.Impl {
    public class AutoLoginTokenAwaitingComponent : Component {
        public AutoLoginTokenAwaitingComponent() { }

        public AutoLoginTokenAwaitingComponent(byte[] passwordDigest) => PasswordDigest = passwordDigest;

        public byte[] PasswordDigest { get; set; }
    }
}