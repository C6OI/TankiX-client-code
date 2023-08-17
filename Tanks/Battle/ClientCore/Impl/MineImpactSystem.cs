using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientGarage.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class MineImpactSystem : ECSSystem {
        [OnEventFire]
        public void Impact(MineImpactEvent e, TankNode tank, MineNode mine) {
            Transform transform = mine.mineInstance.GameObject.transform;

            tank.rigidbody.Rigidbody.AddForceAtPosition(
                transform.up * mine.mineConfig.Impact * WeaponConstants.WEAPON_FORCE_MULTIPLIER,
                transform.position);
        }

        public class TankNode : Node {
            public RigidbodyComponent rigidbody;
            public TankComponent tank;
        }

        public class MineNode : Node {
            public MineComponent mine;

            public MineConfigComponent mineConfig;

            public MineInstanceComponent mineInstance;
        }
    }
}