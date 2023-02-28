using System;
using System.Collections;
using System.Linq;
using Platform.Library.ClientLogger.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public static class PhysicsUtil {
        const float HIT_EPS = 0.001f;

        static readonly RaycastHit[] raycastResults = new RaycastHit[1000];

        public static Vector3 GetPulledHitPoint(RaycastHit hitInfo) {
            Vector3 vector = Vector3.Normalize(hitInfo.normal);
            Vector3 point = hitInfo.point;
            return point + 0.001f * vector;
        }

        public static bool InsiderRaycast(Vector3 origin, Vector3 dir, out RaycastHit hitInfo, float distance, int mask, bool isInsideAttack) {
            Ray ray = new(origin, dir);

            if (!isInsideAttack) {
                return Physics.Raycast(ray, out hitInfo, distance, mask);
            }

            Vector3 point = ray.GetPoint(distance);
            Vector3 direction = -dir;
            int num = Physics.RaycastNonAlloc(point, direction, raycastResults, distance, mask);

            if (num == 0) {
                hitInfo = default;
                return false;
            }

            RaycastHit raycastHit = raycastResults[0];

            for (int i = 0; i < num; i++) {
                RaycastHit raycastHit2 = raycastResults[i];

                if (raycastHit2.distance > raycastHit.distance) {
                    raycastHit = raycastHit2;
                }
            }

            hitInfo = raycastHit;
            return true;
        }

        public static bool RaycastWithExclusion(Vector3 origin, Vector3 dir, out RaycastHit hitInfo, float distance, int mask, GameObject[] exclusions) {
            if (exclusions == null) {
                return Physics.Raycast(origin, dir, out hitInfo, distance, mask);
            }

            int num = Physics.RaycastNonAlloc(origin, dir, raycastResults, distance, mask);

            for (int i = 0; i < num; i++) {
                RaycastHit raycastHit = raycastResults[i];
                GameObject gameObject = raycastHit.collider.gameObject;

                if (gameObject != null && !IsInExclusion(raycastHit.collider.gameObject, exclusions)) {
                    hitInfo = raycastHit;
                    return true;
                }
            }

            hitInfo = default;
            return false;
        }

        static bool IsInExclusion(GameObject go, GameObject[] exclusions) {
            for (int i = 0; i < exclusions.Count(); i++) {
                if (go.Equals(exclusions[i])) {
                    return true;
                }
            }

            return false;
        }

        public static void SetGameObjectLayer(GameObject gameObject, int layer) {
            if (gameObject.GetComponent<IgnoreLayerChanges>() == null) {
                gameObject.layer = layer;
            }

            IEnumerator enumerator = gameObject.transform.GetEnumerator();

            try {
                while (enumerator.MoveNext()) {
                    Transform transform = (Transform)enumerator.Current;
                    SetGameObjectLayer(transform.gameObject, layer);
                }
            } finally {
                IDisposable disposable;

                if ((disposable = enumerator as IDisposable) != null) {
                    disposable.Dispose();
                }
            }
        }

        public static void AddTorqueSafe(this Rigidbody rigidbody, Vector3 torque) {
            if (!IsValidVector3(torque)) {
                LoggerProvider.GetLogger(typeof(PhysicsUtil)).WarnFormat("Invalid add torque (Vector3) StackTrace:[{0}]", Environment.StackTrace);
            } else {
                rigidbody.AddTorque(torque);
            }
        }

        public static void AddTorqueSafe(this Rigidbody rigidbody, float x, float y, float z) {
            if (!IsValidVector3(new Vector3(x, y, z))) {
                LoggerProvider.GetLogger(typeof(PhysicsUtil)).WarnFormat("Invalid add torque (float, float, float) StackTrace:[{0}]", Environment.StackTrace);
            } else {
                rigidbody.AddTorque(x, y, z);
            }
        }

        public static void AddRelativeTorqueSafe(this Rigidbody rigidbody, Vector3 relativeTorque) {
            if (!IsValidVector3(relativeTorque)) {
                LoggerProvider.GetLogger(typeof(PhysicsUtil)).WarnFormat("Invalid add relativeTorque StackTrace:[{0}]", Environment.StackTrace);
            } else {
                rigidbody.AddRelativeTorque(relativeTorque);
            }
        }

        public static void AddForceSafe(this Rigidbody rigidbody, Vector3 force) {
            if (!IsValidVector3(force)) {
                LoggerProvider.GetLogger(typeof(PhysicsUtil)).WarnFormat("Invalid add force (Vector3) StackTrace:[{0}]", Environment.StackTrace);
            } else {
                rigidbody.AddForce(force);
            }
        }

        public static void AddForceAtPositionSafe(this Rigidbody rigidbody, Vector3 force, Vector3 position) {
            if (!IsValidVector3(force)) {
                LoggerProvider.GetLogger(typeof(PhysicsUtil)).WarnFormat("Invalid add force (Vector3, Vector3). Parameter <force> StackTrace:[{0}]", Environment.StackTrace);
            } else if (!IsValidVector3(position)) {
                LoggerProvider.GetLogger(typeof(PhysicsUtil)).WarnFormat("Invalid add force (Vector3, Vector3). Parameter <position> StackTrace:[{0}]", Environment.StackTrace);
            } else {
                rigidbody.AddForceAtPosition(force, position);
            }
        }

        public static void SetVelocitySafe(this Rigidbody rigidbody, Vector3 velocity) {
            if (!IsValidVector3(velocity)) {
                LoggerProvider.GetLogger(typeof(PhysicsUtil)).WarnFormat("Invalid set velocity. StackTrace:[{0}]", Environment.StackTrace);
            } else {
                rigidbody.velocity = velocity;
            }
        }

        public static void SetAngularVelocitySafe(this Rigidbody rigidbody, Vector3 angularVelocity) {
            if (!IsValidVector3(angularVelocity)) {
                LoggerProvider.GetLogger(typeof(PhysicsUtil)).WarnFormat("Invalid set angularVelocity. StackTrace:[{0}]", Environment.StackTrace);
            } else {
                rigidbody.angularVelocity = angularVelocity;
            }
        }

        public static void SetPositionSafe(this Transform transform, Vector3 position) {
            if (!IsValidVector3(position)) {
                LoggerProvider.GetLogger(typeof(PhysicsUtil))
                    .WarnFormat("Invalid set position. transform: {0}, position: {1} StackTrace:[{2}]", transform, position, Environment.StackTrace);
            } else {
                transform.position = position;
            }
        }

        public static void SetRotationSafe(this Transform transform, Quaternion rotation) {
            if (!IsValidQuaternion(rotation)) {
                LoggerProvider.GetLogger(typeof(PhysicsUtil))
                    .WarnFormat("Invalid set rotation. transform: {0}, orientation: {1} StackTrace:[{2}]", transform, rotation, Environment.StackTrace);
            } else {
                transform.rotation = rotation;
            }
        }

        public static void SetLocalPositionSafe(this Transform transform, Vector3 localPosition) {
            if (!IsValidVector3(localPosition)) {
                LoggerProvider.GetLogger(typeof(PhysicsUtil)).WarnFormat("Invalid set localPosition. transform: {0}, localPosition: {1} StackTrace:[{2}]", transform, localPosition,
                    Environment.StackTrace);
            } else {
                transform.localPosition = localPosition;
            }
        }

        public static void SetLocalRotationSafe(this Transform transform, Quaternion localRotation) {
            if (!IsValidQuaternion(localRotation)) {
                LoggerProvider.GetLogger(typeof(PhysicsUtil)).WarnFormat("Invalid set localRotation. transform: {0}, orientation: {1} StackTrace:[{2}]", transform, localRotation,
                    Environment.StackTrace);
            } else {
                transform.localRotation = localRotation;
            }
        }

        public static void SetLocalEulerAnglesSafe(this Transform transform, Vector3 localEulerAngles) {
            if (!IsValidVector3(localEulerAngles)) {
                LoggerProvider.GetLogger(typeof(PhysicsUtil)).WarnFormat("Invalid set localEulerAngles. transform: {0}, localEulerAngles: {1} StackTrace:[{2}]", transform,
                    localEulerAngles, Environment.StackTrace);
            } else {
                transform.localEulerAngles = localEulerAngles;
            }
        }

        public static bool ValidateMovement(Movement m) {
            if (!IsValidQuaternion(m.Orientation)) {
                LoggerProvider.GetLogger(typeof(PhysicsUtil)).WarnFormat("Invalid movement orientation. Movement={0} StackTrace:[{1}]", m, Environment.StackTrace);
                return false;
            }

            if (!IsValidVector3(m.Position)) {
                LoggerProvider.GetLogger(typeof(PhysicsUtil)).WarnFormat("Invalid movement position. Movement={0} StackTrace:[{1}]", m, Environment.StackTrace);
                return false;
            }

            if (!IsValidVector3(m.Velocity)) {
                LoggerProvider.GetLogger(typeof(PhysicsUtil)).WarnFormat("Invalid movement velocity. Movement={0} StackTrace:[{1}]", m, Environment.StackTrace);
                return false;
            }

            if (!IsValidVector3(m.AngularVelocity)) {
                LoggerProvider.GetLogger(typeof(PhysicsUtil)).WarnFormat("Invalid movement angularVelocity. Movement={0} StackTrace:[{1}]", m, Environment.StackTrace);
                return false;
            }

            return true;
        }

        public static bool ValidateVector3(Vector3 v) {
            if (!IsValidVector3(v)) {
                LoggerProvider.GetLogger(typeof(PhysicsUtil)).WarnFormat("Invalid Vector3: {0} StackTrace:[{1}]", v, Environment.StackTrace);
                return false;
            }

            return true;
        }

        public static bool ValidateQuaternion(Quaternion rot) {
            if (!IsValidQuaternion(rot)) {
                LoggerProvider.GetLogger(typeof(PhysicsUtil)).WarnFormat("Invalid Quaternion: {0} StackTrace:[{1}]", rot, Environment.StackTrace);
                return false;
            }

            return true;
        }

        public static bool IsValidVector3(Vector3 v) => IsValidFloat(v.x) && IsValidFloat(v.y) && IsValidFloat(v.z);

        public static bool IsValidQuaternion(Quaternion rot) => IsValidFloat(rot.w) && IsValidFloat(rot.x) && IsValidFloat(rot.y) && IsValidFloat(rot.z);

        public static bool IsValidFloat(float val) => !float.IsInfinity(val) && !float.IsNaN(val);
    }
}