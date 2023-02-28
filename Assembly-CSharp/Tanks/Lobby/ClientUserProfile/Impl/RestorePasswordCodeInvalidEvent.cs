using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientUserProfile.Impl {
    [Shared]
    [SerialVersionUID(1460402823575L)]
    public class RestorePasswordCodeInvalidEvent : Event {
        public string Code { get; set; }
    }
}