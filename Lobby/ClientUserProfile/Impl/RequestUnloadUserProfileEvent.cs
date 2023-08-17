using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientUserProfile.Impl {
    [SerialVersionUID(1451368523887L)]
    [Shared]
    public class RequestUnloadUserProfileEvent : Event {
        public RequestUnloadUserProfileEvent() { }

        public RequestUnloadUserProfileEvent(long userId) => UserId = userId;

        public long UserId { get; set; }
    }
}