using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientFriends.API {
    [Shared]
    [SerialVersionUID(1463641782251L)]
    public class RequestLoadBattleUserForLabelEvent : Event {
        public RequestLoadBattleUserForLabelEvent(Entity sessionEntity) => SessionEntity = sessionEntity;

        public Entity SessionEntity { get; set; }
    }
}