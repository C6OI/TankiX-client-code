using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientUserProfile.Impl {
    [SerialVersionUID(1451368548585L)]
    [Shared]
    public class RequestLoadUserProfileEvent : Event {
        public RequestLoadUserProfileEvent() { }

        public RequestLoadUserProfileEvent(long userId) => UserId = userId;

        public long UserId { get; set; }
    }
}