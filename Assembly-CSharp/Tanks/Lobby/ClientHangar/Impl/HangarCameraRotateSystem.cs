using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientGraphics.Impl;
using UnityEngine;

namespace Tanks.Lobby.ClientHangar.Impl {
    public class HangarCameraRotateSystem : ECSSystem {
        [OnEventFire]
        public void RotateCamera(HangarCameraRotateEvent e, HangarCameraTankViewStateNode hangar) {
            hangar.cameraRootTransform.Root.LookAt(hangar.hangarTankPosition.transform.position);
            hangar.cameraRootTransform.Root.RotateAround(hangar.hangarTankPosition.transform.position, Vector3.up, e.Angle);
        }

        public class HangarCameraTankViewStateNode : Node {
            public CameraComponent camera;

            public CameraRootTransformComponent cameraRootTransform;
            public HangarCameraComponent hangarCamera;

            public HangarCameraRotationEnabledComponent hangarCameraRotationEnabled;

            public HangarCameraTankViewComponent hangarCameraTankView;

            public HangarTankPositionComponent hangarTankPosition;
        }
    }
}