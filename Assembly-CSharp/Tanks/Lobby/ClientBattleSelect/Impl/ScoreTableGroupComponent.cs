using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class ScoreTableGroupComponent : GroupComponent {
        public ScoreTableGroupComponent(Entity keyEntity)
            : base(keyEntity) { }

        public ScoreTableGroupComponent(long key)
            : base(key) { }
    }
}