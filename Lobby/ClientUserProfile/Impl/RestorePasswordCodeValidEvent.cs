using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientUserProfile.Impl {
    [Shared]
    [SerialVersionUID(1460402875430L)]
    public class RestorePasswordCodeValidEvent : Event {
        public string Code { get; set; }
    }
}