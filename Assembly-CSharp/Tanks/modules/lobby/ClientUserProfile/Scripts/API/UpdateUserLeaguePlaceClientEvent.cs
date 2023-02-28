using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace tanks.modules.lobby.ClientUserProfile.Scripts.API {
    [Shared]
    [SerialVersionUID(1505991358304L)]
    public class UpdateUserLeaguePlaceClientEvent : Event {
        public long Place { get; set; }
    }
}