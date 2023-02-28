using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientEntrance.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class RequestToSquadSystem : ECSSystem {
        public class SelfUserNode : Node {
            public SelfUserComponent selfUser;
            public UserComponent user;
        }
    }
}