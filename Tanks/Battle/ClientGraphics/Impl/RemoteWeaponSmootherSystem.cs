using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class RemoteWeaponSmootherSystem : ECSSystem {
        const float SMOOTHING_COEFF = 5f;

        [OnEventFire]
        public void SnapOnAdd(NodeAddedEvent e, WeaponNode weapon, [JoinByTank] [Context] RemoteTankNode tank) =>
            Snap(weapon);

        [OnEventFire]
        public void SnapOnInit(TankMovementInitEvent e, WeaponNode weapon) => Snap(weapon);

        [OnEventComplete]
        public void OnUpdate(TimeUpdateEvent e, WeaponNode node, [JoinByTank] RemoteTankNode tank) {
            Transform transform = node.weaponInstance.WeaponInstance.transform;
            Transform transform2 = node.weaponVisualRoot.transform;
            InterpolateVisualRotation(transform, transform2, e.DeltaTime);
        }

        void Snap(WeaponNode weapon) {
            WeaponVisualRootComponent weaponVisualRoot = weapon.weaponVisualRoot;
            WeaponInstanceComponent weaponInstance = weapon.weaponInstance;
            Transform transform = weaponVisualRoot.transform;
            Transform transform2 = weaponInstance.WeaponInstance.transform;
            transform.localRotation = transform2.localRotation;
        }

        void InterpolateVisualRotation(Transform weaponInstance, Transform visualInstance, float deltaTime) {
            Vector3 localEulerAngles = visualInstance.localEulerAngles;
            float num = CalculateDistance(localEulerAngles.y, weaponInstance);
            float num2 = 5f * deltaTime;

            if (num2 > 1f) {
                num2 = 1f;
            }

            localEulerAngles.y += num * num2;
            visualInstance.localEulerAngles = localEulerAngles;
        }

        float CalculateDistance(float logicValue, Transform weaponInstance) {
            float num = weaponInstance.localEulerAngles.y - logicValue;
            float num2 = 360f - Mathf.Abs(num);

            if (Mathf.Abs(num) > num2) {
                num = !(num > 0f) ? num2 : 0f - num2;
            }

            return num;
        }

        [Not(typeof(DetachedWeaponComponent))]
        public class WeaponNode : Node {
            public TankGroupComponent tankGroup;

            public WeaponInstanceComponent weaponInstance;

            public WeaponVisualRootComponent weaponVisualRoot;
        }

        public class RemoteTankNode : Node {
            public RemoteTankComponent remoteTank;
            public TankGroupComponent tankGroup;
        }
    }
}