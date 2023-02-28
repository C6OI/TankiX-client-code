using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class ClientTankSnapSystem : ECSSystem {
        [OnEventComplete]
        public void InitTimeSmoothing(NodeAddedEvent e, TankNode tank) {
            TransformTimeSmoothingComponent transformTimeSmoothingComponent = new();
            transformTimeSmoothingComponent.Transform = tank.tankVisualRoot.transform;
            transformTimeSmoothingComponent.UseCorrectionByFrameLeader = true;
            tank.Entity.AddComponent(transformTimeSmoothingComponent);
        }

        [OnEventComplete]
        public void UpdateTankPostion(TimeUpdateEvent e, TankNode tank) {
            tank.tankVisualRoot.transform.SetPositionSafe(tank.rigidbody.RigidbodyTransform.position);
            tank.tankVisualRoot.transform.SetRotationSafe(tank.rigidbody.RigidbodyTransform.rotation);
            ScheduleEvent<TransformTimeSmoothingEvent>(tank);
        }

        [OnEventComplete]
        public void UpdateWeaponRotation(UpdateEvent e, WeaponNode weapon, [JoinByTank] TankNode tank) {
            WeaponVisualRootComponent weaponVisualRoot = weapon.weaponVisualRoot;
            WeaponInstanceComponent weaponInstance = weapon.weaponInstance;
            weaponVisualRoot.transform.SetLocalRotationSafe(weaponInstance.WeaponInstance.transform.localRotation);
            weaponVisualRoot.transform.SetLocalPositionSafe(Vector3.zero);
        }

        public class TankNode : Node {
            public RigidbodyComponent rigidbody;

            public SelfTankComponent selfTank;
            public TankGroupComponent tankGroup;

            public TankVisualRootComponent tankVisualRoot;
        }

        [Not(typeof(DetachedWeaponComponent))]
        public class WeaponNode : Node {
            public TankGroupComponent tankGroup;

            public WeaponInstanceComponent weaponInstance;

            public WeaponVisualRootComponent weaponVisualRoot;
        }
    }
}