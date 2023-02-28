using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class DroneGraphicsSystem : ECSSystem {
        [OnEventFire]
        public void InitDroneWeapon(NodeAddedEvent e, [Combine] DroneWeaponNode droneWeapon, [JoinByUnit] [Context] DroneEffectNode droneEffect,
            SingleNode<CameraRootTransformComponent> camera) {
            Renderer renderer = droneEffect.effectRendererGraphics.Renderer;
            CameraVisibleTriggerComponent cameraVisibleTriggerComponent = renderer.gameObject.AddComponent<CameraVisibleTriggerComponent>();
            cameraVisibleTriggerComponent.MainCameraTransform = camera.component.Root;

            if (!droneWeapon.Entity.HasComponent<CameraVisibleTriggerComponent>()) {
                droneWeapon.Entity.AddComponent(cameraVisibleTriggerComponent);
            }
        }

        public class DroneWeaponNode : Node {
            public DroneWeaponComponent droneWeapon;

            public UnitGroupComponent unitGroup;

            public WeaponVisualRootComponent weaponVisualRoot;
        }

        public class DroneEffectNode : Node {
            public DroneEffectComponent droneEffect;

            public EffectRendererGraphicsComponent effectRendererGraphics;

            public RigidbodyComponent rigidbody;

            public UnitGroupComponent unitGroup;
        }
    }
}