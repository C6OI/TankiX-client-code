using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientUserProfile.Impl {
    [Shared]
    [SerialVersionUID(1460402752765L)]
    public class CheckRestorePasswordCodeEvent : Event {
        public string Code { get; set; }
    }
}