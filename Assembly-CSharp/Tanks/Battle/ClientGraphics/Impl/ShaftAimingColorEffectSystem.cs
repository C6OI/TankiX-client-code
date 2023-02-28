using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class ShaftAimingColorEffectSystem : ECSSystem {
        [OnEventFire]
        public void DefineColorForDM(NodeAddedEvent evt, ShaftAimingColorEffectNode weaponNode, [Context] [JoinByBattle] DMBattleNode dm) {
            weaponNode.shaftAimingColorEffect.ChoosenColor = weaponNode.shaftAimingColorEffect.RedColor;
            weaponNode.Entity.AddComponent<ShaftAimingColorEffectPreparedComponent>();
        }

        [OnEventFire]
        public void DefineColorForTeamMode(NodeAddedEvent evt, [Combine] ShaftAimingTeamColorEffectNode weaponNode, [Context] [JoinByTeam] TeamNode team) {
            TeamColor teamColor = team.colorInBattle.TeamColor;
            ShaftAimingColorEffectComponent shaftAimingColorEffect = weaponNode.shaftAimingColorEffect;
            Color choosenColor = teamColor != TeamColor.BLUE ? shaftAimingColorEffect.RedColor : shaftAimingColorEffect.BlueColor;
            shaftAimingColorEffect.ChoosenColor = choosenColor;
            weaponNode.Entity.AddComponent<ShaftAimingColorEffectPreparedComponent>();
        }

        public class ShaftAimingColorEffectNode : Node {
            public BattleGroupComponent battleGroup;
            public ShaftAimingColorEffectComponent shaftAimingColorEffect;
        }

        public class ShaftAimingTeamColorEffectNode : Node {
            public BattleGroupComponent battleGroup;
            public ShaftAimingColorEffectComponent shaftAimingColorEffect;

            public TeamGroupComponent teamGroup;
        }

        public class DMBattleNode : Node {
            public BattleGroupComponent battleGroup;

            public DMComponent dm;
        }

        public class TeamNode : Node {
            public ColorInBattleComponent colorInBattle;

            public TeamGroupComponent teamGroup;
        }
    }
}