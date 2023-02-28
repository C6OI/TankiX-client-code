using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientFriends.API {
    [Shared]
    [SerialVersionUID(1507708322833L)]
    public class FriendRequestSentEvent : Event { }
}