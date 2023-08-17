using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class FlagVisualPickupSystem : ECSSystem {
        static readonly Vector3 FLAG_MOUNT = new(0.5f, 0f, -1f);

        [OnEventFire]
        public void PickupFlag(NodeAddedEvent e, CarriedFlagNode flag, [JoinByTank] [Context] TankNode tank,
            [JoinByTank] WeaponNode weapon) {
            if (flag.teamGroup.Key != tank.teamGroup.Key) {
                GameObject flagInstance = flag.flagInstance.FlagInstance;
                Transform transform = flagInstance.transform;
                transform.parent = weapon.weaponVisualRoot.transform;
                transform.localPosition = FLAG_MOUNT;
            }
        }

        public class CarriedFlagNode : Node {
            public FlagComponent flag;

            public FlagInstanceComponent flagInstance;

            public TankGroupComponent tankGroup;

            public TeamGroupComponent teamGroup;
        }

        public class TankNode : Node {
            public AssembledTankComponent assembledTank;
            public TankComponent tank;

            public TankActiveStateComponent tankActiveState;

            public TankGroupComponent tankGroup;

            public TeamGroupComponent teamGroup;
        }

        public class WeaponNode : Node {
            public TankGroupComponent tankGroup;
            public WeaponComponent weapon;

            public WeaponVisualRootComponent weaponVisualRoot;
        }
    }
}