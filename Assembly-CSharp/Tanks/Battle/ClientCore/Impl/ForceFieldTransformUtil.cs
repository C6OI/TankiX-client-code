using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class ForceFieldTransformUtil {
        public static float RAYCAST_TO_GROUND_MAX_DISTANCE = 3f;

        public static float DISTANCE_FROM_WEAPON = 7.3f;

        public static ForceFieldTranformComponent GetTransformComponent(Transform weaponTransform) {
            Quaternion quaternion = Quaternion.Euler(0f, weaponTransform.rotation.eulerAngles.y, 0f);
            RaycastHit hitInfo;
            ForceFieldTranformComponent forceFieldTranformComponent;

            if (!HitToTheGround(weaponTransform, out hitInfo)) {
                forceFieldTranformComponent = new ForceFieldTranformComponent();

                forceFieldTranformComponent.Movement = new Movement {
                    Position = GetPositionInFrontOfWeapon(weaponTransform),
                    Orientation = quaternion
                };

                return forceFieldTranformComponent;
            }

            Movement movement = default;
            movement.Position = hitInfo.point;
            movement.Orientation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal) * quaternion;
            Movement movement2 = movement;
            forceFieldTranformComponent = new ForceFieldTranformComponent();
            forceFieldTranformComponent.Movement = movement2;
            return forceFieldTranformComponent;
        }

        public static bool CanFallToTheGround(Transform weaponTransform) {
            RaycastHit hitInfo;
            return HitToTheGround(weaponTransform, out hitInfo);
        }

        static bool HitToTheGround(Transform weaponTransform, out RaycastHit hitInfo) {
            Vector3 vector = weaponTransform.position;
            Vector3 vector2 = weaponTransform.TransformDirection(Vector3.forward);
            vector += vector2 * DISTANCE_FROM_WEAPON;
            bool flag = Physics.Raycast(vector, Vector3.down, out hitInfo, RAYCAST_TO_GROUND_MAX_DISTANCE, LayerMasks.STATIC);

            if (!flag) {
                vector2.y = 0f;
                vector = weaponTransform.position + vector2 * DISTANCE_FROM_WEAPON;
                flag = Physics.Raycast(vector, Vector3.down, out hitInfo, RAYCAST_TO_GROUND_MAX_DISTANCE, LayerMasks.STATIC);
            }

            RaycastHit result;
            flag = ImproveHitWithLeftRightHit(flag, hitInfo, vector, out result);
            return ImproveHitWithUpperHit(weaponTransform, flag, result, out hitInfo);
        }

        static bool ImproveHitWithLeftRightHit(bool hitExist, RaycastHit hitInfo, Vector3 position, out RaycastHit result) {
            RaycastHit hitInfo2;
            bool flag = Physics.Raycast(position + Vector3.left * 2.5f, Vector3.down, out hitInfo2, RAYCAST_TO_GROUND_MAX_DISTANCE, LayerMasks.STATIC);
            RaycastHit hitInfo3;
            bool flag2 = Physics.Raycast(position + Vector3.right * 2.5f, Vector3.down, out hitInfo3, RAYCAST_TO_GROUND_MAX_DISTANCE, LayerMasks.STATIC);
            result = hitInfo;

            if (!flag || !flag2) {
                return hitExist;
            }

            if (!hitExist) {
                result = hitInfo2;
                Vector3 point = result.point;
                point.x = position.x;
                point.z = position.z;
                result.point = point;
                return flag;
            }

            if (hitInfo.point.y.Equals(hitInfo2.point.y) || hitInfo.point.y.Equals(hitInfo3.point.y)) {
                return hitExist;
            }

            if (!hitInfo2.point.y.Equals(hitInfo3.point.y)) {
                return hitExist;
            }

            Vector3 point2 = result.point;
            point2.y = hitInfo2.point.y;
            result.point = point2;
            return hitExist;
        }

        static bool ImproveHitWithUpperHit(Transform weaponTransform, bool hitExist, RaycastHit hitInfo, out RaycastHit result) {
            Vector3 position = weaponTransform.position;
            Vector3 vector = weaponTransform.TransformDirection(Vector3.forward);
            position += vector * DISTANCE_FROM_WEAPON;
            position += Vector3.up * RAYCAST_TO_GROUND_MAX_DISTANCE;
            bool hitExist2 = Physics.Raycast(position, Vector3.down, out result, RAYCAST_TO_GROUND_MAX_DISTANCE, LayerMasks.STATIC);
            RaycastHit result2;
            hitExist2 = ImproveHitWithLeftRightHit(hitExist2, result, position, out result2);
            result = hitInfo;

            if (!hitExist) {
                if (hitExist2) {
                    result = result2;
                    return hitExist2;
                }

                return false;
            }

            if (hitExist2) {
                result = !(Vector3.Distance(position, hitInfo.point) < Vector3.Distance(position, result2.point)) ? result2 : hitInfo;
            }

            return true;
        }

        static Vector3 GetPositionInFrontOfWeapon(Transform weaponTransform) {
            Vector3 position = weaponTransform.position;
            return position + weaponTransform.TransformDirection(Vector3.forward) * DISTANCE_FROM_WEAPON;
        }
    }
}