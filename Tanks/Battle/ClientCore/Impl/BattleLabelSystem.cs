using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class BattleLabelSystem : ECSSystem {
        [OnEventFire]
        public void AddUserBattle(NodeAddedEvent e, BattleLabelNode battleLabel, [Context] [JoinByMap] MapNode mapNode) {
            battleLabel.battleLabel.Map = mapNode.mapName.Name;
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
            public MapComponent map;

            public MapGroupComponent mapGroup;

            public MapNameComponent mapName;
        }
    }
}