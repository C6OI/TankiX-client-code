using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class FollowedBattleUserComponent : Component {
        public bool UserHasLeftBattle { get; set; }
    }
}