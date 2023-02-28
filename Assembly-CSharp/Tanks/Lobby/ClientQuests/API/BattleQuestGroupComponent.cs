using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientQuests.API {
    public class BattleQuestGroupComponent : GroupComponent {
        public BattleQuestGroupComponent(Entity keyEntity)
            : base(keyEntity) { }

        public BattleQuestGroupComponent(long key)
            : base(key) { }
    }
}