using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientUserProfile.API {
    [Shared]
    [SerialVersionUID(1503298026299L)]
    public class LeagueGroupComponent : GroupComponent {
        public LeagueGroupComponent(Entity keyEntity)
            : base(keyEntity) { }

        public LeagueGroupComponent(long key)
            : base(key) { }
    }
}