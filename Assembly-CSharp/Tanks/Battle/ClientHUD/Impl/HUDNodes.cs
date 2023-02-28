using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Battle.ClientHUD.Impl {
    public class HUDNodes {
        public class SelfTankNode : Node {
            public HealthComponent health;

            public HealthConfigComponent healthConfig;

            public SelfTankComponent selfTank;
            public TankComponent tank;

            public TankGroupComponent tankGroup;
        }

        public class BattleUserNode : Node {
            public BattleGroupComponent battleGroup;

            public UserGroupComponent userGroup;
        }

        public class SelfBattleUserNode : BattleUserNode {
            public SelfBattleUserComponent selfBattleUser;
        }

        public class ActiveSelfTankNode : SelfTankNode {
            public TankActiveStateComponent tankActiveState;
        }

        public class DeadSelfTankNode : SelfTankNode {
            public TankDeadStateComponent tankDeadState;
        }

        public class SemiActiveSelfTankNode : SelfTankNode {
            public TankSemiActiveStateComponent tankSemiActiveState;
        }

        public abstract class BaseWeaponNode : Node {
            public TankGroupComponent tankGroup;
            public WeaponComponent weapon;

            public WeaponEnergyComponent weaponEnergy;
        }

        public class SelfBattleUserAsSpectatorNode : SelfBattleUserNode {
            public UserInBattleAsSpectatorComponent userInBattleAsSpectator;
        }

        public class SelfBattleUserAsTankNode : SelfBattleUserNode {
            public UserInBattleAsTankComponent userInBattleAsTank;
        }

        public class Modules {
            public class SlotNode : Node {
                public SlotUserItemInfoComponent slotUserItemInfo;

                public TankGroupComponent tankGroup;
            }

            public class SlotWithModuleNode : SlotNode {
                public ModuleGroupComponent moduleGroup;
            }
        }
    }
}