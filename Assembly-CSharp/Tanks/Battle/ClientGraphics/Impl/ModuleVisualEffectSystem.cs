using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using UnityEngine;
using Event = Platform.Kernel.ECS.ClientEntitySystem.API.Event;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class ModuleVisualEffectSystem : ECSSystem {
        [OnEventFire]
        public void ScheduleInitEffects(NodeAddedEvent e, TankInstanceNode tank) {
            NewEvent<InitEffectsEvent>().Attach(tank).ScheduleDelayed(0.3f);
        }

        [OnEventFire]
        public void InitEffects(InitEffectsEvent e, TankInstanceNode tank) {
            Transform transform = new GameObject("ModuleVisualEffects").transform;
            transform.SetParent(tank.tankCommonInstance.TankCommonInstance.transform);
            transform.localPosition = Vector3.zero;
            GameObject gameObject = Object.Instantiate(tank.moduleVisualEffectPrefabs.JumpImpactEffectPrefab, transform);
            gameObject.SetActive(false);
            GameObject gameObject2 = Object.Instantiate(tank.moduleVisualEffectPrefabs.ExternalImpactEffectPrefab, transform);
            gameObject2.SetActive(false);
            GameObject gameObject3 = Object.Instantiate(tank.moduleVisualEffectPrefabs.FireRingEffectPrefab, transform);
            gameObject3.SetActive(false);
            GameObject gameObject4 = Object.Instantiate(tank.moduleVisualEffectPrefabs.ExplosiveMassEffectPrefab, transform);
            gameObject4.SetActive(false);
            ModuleVisualEffectObjectsComponent moduleVisualEffectObjectsComponent = new();
            moduleVisualEffectObjectsComponent.JumpImpactEffect = gameObject;
            moduleVisualEffectObjectsComponent.ExternalImpactEffect = gameObject2;
            moduleVisualEffectObjectsComponent.FireRingEffect = gameObject3;
            moduleVisualEffectObjectsComponent.ExplosiveMassEffect = gameObject4;
            ModuleVisualEffectObjectsComponent component = moduleVisualEffectObjectsComponent;
            tank.Entity.AddComponent(component);
        }

        [OnEventFire]
        public void TurnOffEffectsOnDeath(NodeRemoveEvent e, TankNode tank) {
            tank.moduleVisualEffectObjects.JumpImpactEffect.SetActive(false);
            tank.moduleVisualEffectObjects.ExternalImpactEffect.SetActive(false);
            tank.moduleVisualEffectObjects.FireRingEffect.SetActive(false);
            tank.moduleVisualEffectObjects.ExplosiveMassEffect.SetActive(false);
        }

        public class TankInstanceNode : Node {
            public ModuleVisualEffectPrefabsComponent moduleVisualEffectPrefabs;
            public TankCommonInstanceComponent tankCommonInstance;
        }

        public class TankNode : Node {
            public AssembledTankActivatedStateComponent assembledTankActivatedState;

            public BaseRendererComponent baseRenderer;

            public BattleGroupComponent battleGroup;

            public ModuleVisualEffectObjectsComponent moduleVisualEffectObjects;

            public RigidbodyComponent rigidbody;

            public TankActiveStateComponent tankActiveState;

            public TankCollidersComponent tankColliders;

            public TankGroupComponent tankGroup;
        }

        public class InitEffectsEvent : Event { }
    }
}