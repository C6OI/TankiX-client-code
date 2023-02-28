using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class VulcanKickbackSystem : ECSSystem {
        [OnEventFire]
        public void ApplyKickback(FixedUpdateEvent evt, KickbackNode weapon, [JoinByTank] TankNode tank) {
            KickbackComponent kickback = weapon.kickback;
            MuzzleLogicAccessor muzzleLogicAccessor = new(weapon.muzzlePoint, weapon.weaponInstance);
            float deltaTime = evt.DeltaTime;
            Vector3 force = -muzzleLogicAccessor.GetFireDirectionWorld() * kickback.KickbackForce * WeaponConstants.WEAPON_FORCE_MULTIPLIER * deltaTime;
            Vector3 worldMiddlePosition = muzzleLogicAccessor.GetWorldMiddlePosition();
            Rigidbody rigidbody = tank.rigidbody.Rigidbody;
            TrackComponent track = tank.track;
            TankFallingComponent tankFalling = tank.tankFalling;
            VulcanPhysicsUtils.ApplyVulcanForce(force, rigidbody, worldMiddlePosition, tankFalling, track);
        }

        public class KickbackNode : Node {
            public KickbackComponent kickback;

            public MuzzlePointComponent muzzlePoint;

            public TankGroupComponent tankGroup;

            public WeaponInstanceComponent weaponInstance;

            public WeaponStreamShootingComponent weaponStreamShooting;
        }

        public class TankNode : Node {
            public RigidbodyComponent rigidbody;

            public TankFallingComponent tankFalling;
            public TankGroupComponent tankGroup;

            public TrackComponent track;
        }
    }
}