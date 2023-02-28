using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class AcceleratedGearsEffectSystem : ECSSystem {
        [OnEventFire]
        public void Instantiate(NodeAddedEvent e, SingleNode<PreloadedModuleEffectsComponent> mapEffect, [Combine] EffectReadyNode effect, [JoinByTank] [Context] TankNode tank) {
            if (!tank.Entity.HasComponent<AcceleratedGearsInstanceComponent>()) {
                GameObject gameObject = mapEffect.component.PreloadedEffects["acceleratedgears"];

                if ((bool)gameObject) {
                    GameObject gameObject2 = Object.Instantiate(gameObject);
                    gameObject2.SetActive(true);
                    gameObject2.transform.SetParent(tank.mountPoint.MountPoint, false);
                    gameObject2.transform.localPosition = Vector3.zero;
                    tank.Entity.AddComponent(new AcceleratedGearsInstanceComponent(gameObject2));
                }
            }
        }

        [OnEventFire]
        public void UpdateEffect(TimeUpdateEvent e, TankWithGearsNode tank, [JoinByTank] WeaponNode weapon, [JoinByTank] EffectReadyNode effect) {
            tank.acceleratedGearsInstance.Instance.SetActive(weapon.weaponRotationControl.Speed > weapon.weaponRotation.BaseSpeed && weapon.weaponRotationControl.EffectiveControl != 0f);
        }

        [OnEventFire]
        public void StopEffect(NodeRemoveEvent e, EffectReadyNode effect, [JoinByTank] TankWithGearsNode tank) {
            tank.acceleratedGearsInstance.Instance.SetActive(false);
        }

        public class EffectReadyNode : Node {
            public AcceleratedGearsEffectComponent acceleratedGearsEffect;

            public TankGroupComponent tankGroup;
        }

        [Not(typeof(AcceleratedGearsInstanceComponent))]
        public class TankNode : Node {
            public MountPointComponent mountPoint;

            public TankGroupComponent tankGroup;
        }

        public class TankWithGearsNode : Node {
            public AcceleratedGearsInstanceComponent acceleratedGearsInstance;
        }

        public class WeaponNode : Node {
            public WeaponRotationComponent weaponRotation;

            public WeaponRotationControlComponent weaponRotationControl;
        }
    }
}