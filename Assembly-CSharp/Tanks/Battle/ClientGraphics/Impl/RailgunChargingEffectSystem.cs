using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class RailgunChargingEffectSystem : ECSSystem {
        const string RAILGUN_CHARGING_ENTITY_NAME = "railgun_charging";

        [OnEventFire]
        [Mandatory]
        public void StartRailgunCharging(SelfRailgunChargingShotEvent evt, RailgunChargingNode muzzle, [JoinBy(typeof(TankGroupComponent))] TankActiveNode tank) {
            StartRailgunChargingByBaseEvent(evt, muzzle, tank);
        }

        [OnEventFire]
        public void StartRailgunCharging(RemoteRailgunChargingShotEvent evt, RailgunChargingNode muzzle, [JoinBy(typeof(TankGroupComponent))] TankActiveNode tank) {
            StartRailgunChargingByBaseEvent(evt, muzzle, tank);
        }

        void StartRailgunChargingByBaseEvent(BaseRailgunChargingShotEvent evt, RailgunChargingNode muzzle, TankActiveNode tank) {
            RailgunChargingWeaponComponent railgunChargingWeapon = muzzle.railgunChargingWeapon;
            RailgunChargingEffectComponent railgunChargingEffect = muzzle.railgunChargingEffect;
            GetInstanceFromPoolEvent getInstanceFromPoolEvent = new();
            getInstanceFromPoolEvent.Prefab = railgunChargingEffect.Prefab;
            getInstanceFromPoolEvent.AutoRecycleTime = railgunChargingWeapon.ChargingTime;
            GetInstanceFromPoolEvent getInstanceFromPoolEvent2 = getInstanceFromPoolEvent;
            ScheduleEvent(getInstanceFromPoolEvent2, muzzle);
            Transform instance = getInstanceFromPoolEvent2.Instance;
            GameObject gameObject = instance.gameObject;
            CustomRenderQueue.SetQueue(gameObject, 3150);
            ParticleSystem component = gameObject.GetComponent<ParticleSystem>();
            ParticleSystem.MainModule main = component.main;
            main.startLifetime = railgunChargingWeapon.ChargingTime;
            ParticleSystem.EmissionModule emission = component.emission;
            emission.enabled = true;
            UnityUtil.InheritAndEmplace(instance, muzzle.muzzlePoint.Current);
            gameObject.SetActive(true);
            Entity entity = CreateEntity("railgun_charging");
            entity.AddComponent(new TankGroupComponent(tank.Entity));
        }

        [OnEventFire]
        public void DestroyWeaponChargingEffect(NodeRemoveEvent evt, ChargingGraphicsNode effect) {
            DeleteEntity(effect.Entity);
        }

        public class RailgunChargingNode : Node {
            public MuzzlePointComponent muzzlePoint;

            public RailgunChargingEffectComponent railgunChargingEffect;

            public RailgunChargingWeaponComponent railgunChargingWeapon;
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