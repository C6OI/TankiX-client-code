using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientGraphics.API;

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
            tank.tankVisualRoot.transform.position = tank.rigidbody.Rigidbody.transform.position;
            tank.tankVisualRoot.transform.rotation = tank.rigidbody.Rigidbody.transform.rotation;
            ScheduleEvent<TransformTimeSmoothingEvent>(tank);
        }

        [OnEventComplete]
        public void UpdateWeaponRotation(UpdateEvent e, WeaponNode weapon, [JoinByTank] TankNode tank) {
            WeaponVisualRootComponent weaponVisualRoot = weapon.weaponVisualRoot;
            WeaponInstanceComponent weaponInstance = weapon.weaponInstance;
            weaponVisualRoot.transform.localRotation = weaponInstance.WeaponInstance.transform.localRotation;
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