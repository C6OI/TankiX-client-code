using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class KickbackSystem : ECSSystem {
        [OnEventFire]
        public void StartKickback(BaseShotEvent evt, KickbackNode weapon, [JoinByTank] TankNode tank) {
            KickbackComponent kickback = weapon.kickback;
            MuzzleLogicAccessor muzzleLogicAccessor = new(weapon.muzzlePoint, weapon.weaponInstance);
            Vector3 vector = -muzzleLogicAccessor.GetFireDirectionWorld() * kickback.KickbackForce;
            Vector3 worldPosition = muzzleLogicAccessor.GetWorldPosition();
            tank.rigidbody.Rigidbody.AddForceAtPositionSafe(vector * WeaponConstants.WEAPON_FORCE_MULTIPLIER, worldPosition);
        }

        public class KickbackNode : Node {
            public DiscreteWeaponComponent discreteWeapon;
            public KickbackComponent kickback;

            public MuzzlePointComponent muzzlePoint;

            public TankGroupComponent tankGroup;

            public WeaponInstanceComponent weaponInstance;
        }

        public class TankNode : Node {
            public RigidbodyComponent rigidbody;
            public TankComponent tank;

            public TankGroupComponent tankGroup;
        }
    }
}