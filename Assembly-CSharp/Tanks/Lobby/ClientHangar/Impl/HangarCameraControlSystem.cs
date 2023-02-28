using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientGraphics.Impl;
using Tanks.Lobby.ClientControls.API;
using UnityEngine;

namespace Tanks.Lobby.ClientHangar.Impl {
    public class HangarCameraControlSystem : ECSSystem {
        const float MIN_ROTATION_ANGLE = 0.1f;

        const float MAX_ROTATION_SPEED = 1080f;

        [OnEventFire]
        public void MouseRotateBegin(EventSystemOnBeginDragEvent e, ScreenForegroundNode screenForeground, [JoinAll] HangarCameraTankViewNonDragNode hangar) {
            hangar.Entity.AddComponent<HangarCameraDragComponent>();
        }

        [OnEventFire]
        public void MouseRotateBegin(EventSystemOnBeginDragEvent e, ScreenForegroundNode screenForeground, [JoinAll] HangarCameraTankViewIdleNode hangar) {
            hangar.Entity.AddComponent<HangarCameraDecelerationRotateComponent>();
        }

        [OnEventFire]
        public void MouseRotate(EventSystemOnDragEvent e, ScreenForegroundNode screenForeground, [JoinAll] HangarCameraTankViewRotateNode hangar) {
            HangarCameraRotateEvent hangarCameraRotateEvent = new();
            float num2 = hangarCameraRotateEvent.Angle = e.PointerEventData.delta.x * hangar.hangarConfig.MouseRotateFactor;
            hangar.hangarCameraDecelerationRotate.Speed = num2 / Time.deltaTime;
            hangar.hangarCameraDecelerationRotate.LastUpdateFrame = Time.frameCount;
            ScheduleEvent(hangarCameraRotateEvent, hangar);
        }

        [OnEventFire]
        public void MouseRotateEnd(EventSystemOnEndDragEvent e, ScreenForegroundNode screenForeground, [JoinAll] HangarCameraTankViewRotateNode hangar) {
            hangar.Entity.RemoveComponent<HangarCameraDragComponent>();

            if (Time.frameCount - hangar.hangarCameraDecelerationRotate.LastUpdateFrame > 1) {
                hangar.Entity.RemoveComponent<HangarCameraDecelerationRotateComponent>();
            } else if (hangar.hangarCameraDecelerationRotate.Speed > 1080f) {
                hangar.hangarCameraDecelerationRotate.Speed = 1080f;
            }
        }

        [OnEventFire]
        public void DecelerationRotate(UpdateEvent e, HangarCameraTankViewDecelerationRotateNode hangar) {
            hangar.hangarCameraDecelerationRotate.Speed *= Mathf.Exp((0f - hangar.hangarConfig.DecelerationRotateFactor) * Time.deltaTime);
            float num = hangar.hangarCameraDecelerationRotate.Speed * Time.deltaTime;

            if (Mathf.Abs(num) < 0.1f) {
                hangar.Entity.RemoveComponent<HangarCameraDecelerationRotateComponent>();
            } else {
                ScheduleEvent(new HangarCameraRotateEvent(num), hangar);
            }
        }

        [OnEventFire]
        public void StopDecelerationRotate(EventSystemOnPointerDownEvent e, ScreenForegroundNode screenForeground, [JoinAll] HangarCameraTankViewDecelerationRotateNode hangar) {
            hangar.Entity.RemoveComponent<HangarCameraDecelerationRotateComponent>();
        }

        [OnEventFire]
        public void StopDecelerationRotate(NodeRemoveEvent e, HangarCameraTankViewStateNode hangar, [JoinAll] HangarCameraTankViewDecelerationRotateNode hangarRotate) {
            hangarRotate.Entity.RemoveComponent<HangarCameraDecelerationRotateComponent>();
        }

        public class ScreenForegroundNode : Node {
            public EventSystemProviderComponent eventSystemProvider;
            public ScreenForegroundComponent screenForeground;
        }

        public class HangarCameraTankViewStateNode : Node {
            public CameraComponent camera;
            public HangarCameraComponent hangarCamera;

            public HangarCameraTankViewComponent hangarCameraTankView;

            public HangarConfigComponent hangarConfig;

            public HangarTankPositionComponent hangarTankPosition;
        }

        public class HangarCameraTankViewRotateNode : HangarCameraTankViewStateNode {
            public HangarCameraDecelerationRotateComponent hangarCameraDecelerationRotate;
            public HangarCameraDragComponent hangarCameraDrag;
        }

        [Not(typeof(HangarCameraDragComponent))]
        public class HangarCameraTankViewDecelerationRotateNode : HangarCameraTankViewStateNode {
            public HangarCameraDecelerationRotateComponent hangarCameraDecelerationRotate;
        }

        [Not(typeof(HangarCameraDecelerationRotateComponent))]
        public class HangarCameraTankViewIdleNode : HangarCameraTankViewStateNode { }

        [Not(typeof(HangarCameraDragComponent))]
        public class HangarCameraTankViewNonDragNode : HangarCameraTankViewStateNode { }
    }
}