using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientHUD.Impl {
    public class NameplatePositioningSystem : ECSSystem {
        const float REPOSITION_THRESHOLD = 1.2f;

        [OnEventFire]
        public void UpdateNameplateTransform(UpdateEvent e, NameplateNode nameplate, [JoinByTank] WeaponRendererNode weapon, [JoinByTank] TankNode remoteTank,
            [JoinAll] SingleNode<HUDWorldSpaceCanvas> worldSpaceHUD) {
            NameplateComponent nameplate2 = nameplate.nameplate;
            Transform transform = nameplate2.transform;
            Camera cachedCamera = nameplate2.CachedCamera;
            Vector3 position = weapon.weaponVisualRoot.transform.position;
            PositionAboveTank(position, transform, nameplate2);
            AlignToCamera(nameplate, transform, cachedCamera);
            WorldSpaceHUDUtil.ScaleToRealSize(worldSpaceHUD.component.canvas.transform, transform, nameplate2.CachedCamera);
            nameplate.nameplatePosition.sqrDistance = (cachedCamera.transform.position - transform.position).sqrMagnitude;
        }

        void AlignToCamera(NameplateNode nameplate, Transform nameplateTransform, Camera camera) {
            Vector3 vector = camera.WorldToScreenPoint(nameplateTransform.position);
            Vector3 previousPosition = nameplate.nameplatePosition.previousPosition;
            float x = Mathf.Round(vector.x);
            float y = Mathf.Round(vector.y);
            float z = vector.z;

            if (NearlyEqual(vector, previousPosition)) {
                vector.x = Mathf.Round(previousPosition.x);
                vector.y = Mathf.Round(previousPosition.y);
            } else {
                vector.x = Mathf.Round(vector.x);
                vector.y = Mathf.Round(vector.y);
            }

            nameplate.nameplatePosition.previousPosition = vector;
            Vector3 position = new(x, y, z);
            nameplateTransform.position = camera.ScreenToWorldPoint(position);
            nameplateTransform.rotation = camera.transform.rotation;
        }

        bool NearlyEqual(Vector3 inCamPos, Vector3 previousPos) => Mathf.Abs(inCamPos.x - previousPos.x) <= 1.2f && Mathf.Abs(inCamPos.y - previousPos.y) <= 1.2f;

        void PositionAboveTank(Vector3 position, Transform nameplateTransform, NameplateComponent nameplateComponent) {
            float x = position.x;
            float y = position.y + nameplateComponent.yOffset;
            float z = position.z;
            nameplateTransform.position = new Vector3(x, y, z);
        }

        public class WeaponRendererNode : Node {
            public TankGroupComponent tankGroup;
            public WeaponComponent weapon;

            public WeaponVisualRootComponent weaponVisualRoot;
        }

        public class NameplateNode : Node {
            public NameplateComponent nameplate;

            public NameplatePositionComponent nameplatePosition;

            public TankGroupComponent tankGroup;
        }

        public class TankNode : Node {
            public RemoteTankComponent remoteTank;
            public TankGroupComponent tankGroup;

            public TankVisualRootComponent tankVisualRoot;
        }
    }
}