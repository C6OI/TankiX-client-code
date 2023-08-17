using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientHUD.Impl {
    public class NameplatePositioningSystem : ECSSystem {
        const float REPOSITION_THRESHOLD = 1.2f;

        [OnEventFire]
        public void UpdateNameplateTransform(UpdateEvent e, NameplateNode nameplate, [JoinByTank] WeaponRendererNode weapon,
            [JoinByTank] TankNode remoteTank, [JoinAll] SingleNode<HUDWorldSpaceCanvas> worldSpaceHUD) {
            NameplateComponent nameplate2 = nameplate.nameplate;
            Transform transform = nameplate2.transform;
            Vector3 position = weapon.weaponVisualRoot.transform.position;
            PositionAboveTank(position, transform, nameplate2);
            AlignToCamera(nameplate, transform);
            UpdateScale(worldSpaceHUD, transform);
            nameplate.nameplatePosition.sqrDistance = (Camera.main.transform.position - transform.position).sqrMagnitude;
        }

        void AlignToCamera(NameplateNode nameplate, Transform nameplateTransform) {
            Vector3 vector = Camera.main.WorldToScreenPoint(nameplateTransform.position);
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
            nameplateTransform.position = Camera.main.ScreenToWorldPoint(position);
            nameplateTransform.rotation = Camera.main.transform.rotation;
        }

        bool NearlyEqual(Vector3 inCamPos, Vector3 previousPos) =>
            Mathf.Abs(inCamPos.x - previousPos.x) <= 1.2f && Mathf.Abs(inCamPos.y - previousPos.y) <= 1.2f;

        void PositionAboveTank(Vector3 position, Transform nameplateTransform, NameplateComponent nameplateComponent) {
            float x = position.x;
            float y = position.y + nameplateComponent.yOffset;
            float z = position.z;
            nameplateTransform.position = new Vector3(x, y, z);
        }

        void UpdateScale(SingleNode<HUDWorldSpaceCanvas> worldSpaceHUD, Transform nameplateTransform) {
            float num = Camera.main.pixelHeight / (2f * Mathf.Tan((float)Math.PI / 360f * Camera.main.fieldOfView));
            Vector3 vector = Camera.main.WorldToViewportPoint(nameplateTransform.position);
            float num2 = 1f / worldSpaceHUD.component.canvas.transform.localScale.x;
            float num3 = vector.z / num * num2;
            nameplateTransform.localScale = new Vector3(num3, num3, num3);
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