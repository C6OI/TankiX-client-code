using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class RailgunChargingEffectSystem : ECSSystem {
        const string RAILGUN_CHARGING_ENTITY_NAME = "railgun_charging";

        [OnEventFire]
        public void StartRailgunCharging(SelfRailgunChargingShotEvent evt, RailgunChargingNode muzzle,
            [JoinBy(typeof(TankGroupComponent))] TankActiveNode tank) => StartRailgunChargingByBaseEvent(evt, muzzle, tank);

        [OnEventFire]
        public void StartRailgunCharging(RemoteRailgunChargingShotEvent evt, RailgunChargingNode muzzle,
            [JoinBy(typeof(TankGroupComponent))] TankActiveNode tank) => StartRailgunChargingByBaseEvent(evt, muzzle, tank);

        void StartRailgunChargingByBaseEvent(BaseRailgunChargingShotEvent evt, RailgunChargingNode muzzle,
            TankActiveNode tank) {
            RailgunChargingWeaponComponent railgunChargingWeapon = muzzle.railgunChargingWeapon;
            RailgunChargingEffectComponent railgunChargingEffect = muzzle.railgunChargingEffect;
            GameObject gameObject = Object.Instantiate(railgunChargingEffect.Prefab);
            ParticleSystem component = gameObject.GetComponent<ParticleSystem>();
            component.startLifetime = railgunChargingWeapon.ChargingTime;
            component.enableEmission = true;
            UnityUtil.InheritAndEmplace(gameObject.transform, muzzle.muzzlePoint.Current);
            Entity entity = CreateEntity("railgun_charging");
            entity.AddComponent(new TankGroupComponent(tank.Entity));
            Object.Destroy(gameObject, railgunChargingWeapon.ChargingTime);
        }

        [OnEventFire]
        public void DestroyWeaponChargingEffect(NodeRemoveEvent evt, ChargingGraphicsNode effect) =>
            DeleteEntity(effect.Entity);

        public class RailgunChargingNode : Node {
            public MuzzlePointComponent muzzlePoint;

            public RailgunChargingEffectComponent railgunChargingEffect;

            public RailgunChargingWeaponComponent railgunChargingWeapon;

            public WeaponUnblockedComponent weaponUnblocked;
        }

        public class TankActiveNode : Node {
            public TankActiveStateComponent tankActiveState;
        }

        public class ChargingGraphicsNode : Node {
            public InstanceDestructionComponent instanceDestruction;
            public TankGroupComponent tankGroup;
        }
    }
}