using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientUserProfile.Impl {
    [Shared]
    [SerialVersionUID(1487160556894L)]
    public class ShowNotificationGroupEvent : Event {
        public int ExpectedMembersCount { get; set; }
    }
}