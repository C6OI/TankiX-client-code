using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class BattleLabelSystem : ECSSystem {
        [OnEventFire]
        public void AddUserBattle(NodeAddedEvent e, BattleLabelNode battleLabel, [JoinByMap] [Context] MapNode mapNode) {
            battleLabel.battleLabel.Map = mapNode.descriptionItem.Name;
            battleLabel.battleLabel.Mode = battleLabel.battleInfoForLabel.BattleMode;
            battleLabel.battleLabel.BattleIconActivity = true;
        }

        public class BattleLabelNode : Node {
            public BattleInfoForLabelComponent battleInfoForLabel;
            public BattleLabelComponent battleLabel;

            public MapGroupComponent mapGroup;
        }

        public class Battle : Node {
            public BattleComponent battle;

            public BattleGroupComponent battleGroup;

            public BattleModeComponent battleMode;

            public MapGroupComponent mapGroup;
        }

        public class MapNode : Node {
            public DescriptionItemComponent descriptionItem;
            public MapComponent map;

            public MapGroupComponent mapGroup;
        }
    }
}