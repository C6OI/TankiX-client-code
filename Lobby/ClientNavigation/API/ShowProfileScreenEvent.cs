using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Lobby.ClientNavigation.API {
    public class ShowProfileScreenEvent : Event {
        public ShowProfileScreenEvent(long userId) => UserId = userId;

        public long UserId { get; set; }
    }
}