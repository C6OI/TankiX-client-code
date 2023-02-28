using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientProfile.API;

namespace Tanks.Lobby.ClientUserProfile.Impl {
    public class GetUserLevelInfoEvent : Event {
        public LevelInfo Info { get; set; }
    }
}