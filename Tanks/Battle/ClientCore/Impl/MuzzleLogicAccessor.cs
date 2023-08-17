using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public struct MuzzleLogicAccessor {
        readonly MuzzlePointComponent muzzleComponent;

        readonly Transform weaponTransform;

        public MuzzleLogicAccessor(MuzzlePointComponent muzzleComponent, WeaponInstanceComponent weaponInstanceComponent) {
            this.muzzleComponent = muzzleComponent;
            weaponTransform = weaponInstanceComponent.WeaponInstance.transform;
        }

        public int GetCurrentIndex() => muzzleComponent.CurrentIndex;

        public Vector3 GetWorldPosition() => weaponTransform.TransformPoint(muzzleComponent.Current.localPosition);

        public Vector3 GetBarrelOriginWorld() {
            Vector3 localPosition = muzzleComponent.Current.localPosition;
            localPosition.z = 0f;
            return weaponTransform.TransformPoint(localPosition);
        }

        public Vector3 GetWorldPositionShiftDirectionBarrel(float shiftValue) {
            Vector3 localPosition = muzzleComponent.Current.localPosition;
            localPosition.z *= shiftValue;
            return weaponTransform.TransformPoint(localPosition);
        }

        public Vector3 GetWorldMiddlePosition() {
            Vector3 zero = Vector3.zero;
            Transform[] points = muzzleComponent.Points;

            foreach (Transform transform in points) {
                zero += muzzleComponent.Current.localPosition;
            }

            return weaponTransform.TransformPoint(zero / muzzleComponent.Points.Length);
        }

        public Vector3 GetFireDirectionWorld() => weaponTransform.forward;

        public Vector3 GetLeftDirectionWorld() => -weaponTransform.right;

        public Vector3 GetUpDirectionWorld() => weaponTransform.up;
    }
}