using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientEntrance.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class MapPhysicsSystem : ECSSystem {
        [OnEventFire]
        public void InitGravity(NodeAddedEvent e, SelfBattleUserNode selfBattleUser, [JoinByBattle] [Mandatory] BattleNode battle) {
            Physics.gravity = Vector3.down * battle.gravity.Gravity;
        }

        public class SelfBattleUserNode : Node {
            public BattleGroupComponent battleGroup;

            public BattleUserComponent battleUser;
            public SelfComponent self;
        }

        public class BattleNode : Node {
            public BattleComponent battle;

            public BattleGroupComponent battleGroup;

            public GravityComponent gravity;

            public MapGroupComponent mapGroup;
        }
    }
}