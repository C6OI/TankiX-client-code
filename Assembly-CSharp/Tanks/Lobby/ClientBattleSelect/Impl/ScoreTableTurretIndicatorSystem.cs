using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientBattleSelect.API;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class ScoreTableTurretIndicatorSystem : ECSSystem {
        [OnEventFire]
        public void SetTurrets(NodeAddedEvent e, [Combine] TurretIndicatorNode turretIndicator, [Context] [JoinByUser] UserWeaponNode userWeapon) {
            SetTurret(turretIndicator, userWeapon);
        }

        void SetTurret(TurretIndicatorNode hullIndicator, UserWeaponNode userWeapon) {
            hullIndicator.scoreTableTurretIndicator.SetTurretIcon(userWeapon.marketItemGroup.Key);
        }

        public class TurretIndicatorNode : Node {
            public ScoreTableTurretIndicatorComponent scoreTableTurretIndicator;

            public UserGroupComponent userGroup;
        }

        public class UserWeaponNode : Node {
            public MarketItemGroupComponent marketItemGroup;

            public TankPartComponent tankPart;
            public UserGroupComponent userGroup;

            public WeaponComponent weapon;
        }
    }
}