using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientGraphics.Impl;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Lobby.ClientHangar.Impl {
    public class HangarCameraAutoRotateSystem : ECSSystem {
        [OnEventFire]
        public void StartSchedule(NodeAddedEvent e, HangarCameraTankViewStateRotationEnabledNode hangar) {
            ScheduledEvent scheduledEvent = NewEvent<HangarCameraStartAutoRotateEvent>().Attach(hangar).ScheduleDelayed(hangar.hangarConfig.AutoRotateDelay);
            hangar.Entity.AddComponent(new HangarCameraRotateScheduledComponent(scheduledEvent));
        }

        [OnEventFire]
        public void DisableSchedule(NodeRemoveEvent e, HangarCameraTankViewStateRotationEnabledNode nr, [JoinSelf] HangarCameraRotateScheduledTankViewStateNode hangar) {
            hangar.hangarCameraRotateScheduled.ScheduledEvent.Manager().Cancel();
            hangar.Entity.RemoveComponent<HangarCameraRotateScheduledComponent>();
        }

        [OnEventFire]
        public void DisableAutoRotation(NodeRemoveEvent e, HangarCameraTankViewStateRotationEnabledNode nr, [JoinSelf] HangarCameraAutoRotateNode hangar) {
            hangar.hangarCameraRotateScheduled.ScheduledEvent.Manager().Cancel();
            hangar.Entity.RemoveComponent<HangarCameraAutoRotateComponent>();
        }

        [OnEventFire]
        public void StartRotate(HangarCameraStartAutoRotateEvent e, HangarCameraRotateScheduledTankViewStateNode hangar) {
            hangar.hangarCameraRotateScheduled.ScheduledEvent.Manager().Cancel();
            hangar.Entity.AddComponent<HangarCameraAutoRotateComponent>();
        }

        [OnEventFire]
        public void RotateCamera(TimeUpdateEvent e, HangarCameraAutoRotateNode hangar) {
            ScheduleEvent(new HangarCameraRotateEvent(e.DeltaTime * hangar.hangarConfig.AutoRotateSpeed), hangar);
        }

        [OnEventFire]
        public void CheckUserActionOnAnyPointerEvent(EventSystemPointerEvent e, SingleNode<ScreenForegroundComponent> foreground,
            [JoinAll] HangarCameraRotateScheduledTankViewStateNode hangar) {
            ScheduleEvent<HangarCameraDelayAutoRotateEvent>(hangar);
        }

        [OnEventFire]
        public void DelayScheduledEvent(HangarCameraDelayAutoRotateEvent e, Node any, [JoinAll] HangarCameraRotateScheduledTankViewStateNode hangar) {
            hangar.hangarCameraRotateScheduled.ScheduledEvent.Manager().Cancel();
            hangar.hangarCameraRotateScheduled.ScheduledEvent = NewEvent<HangarCameraStartAutoRotateEvent>().Attach(hangar).ScheduleDelayed(hangar.hangarConfig.AutoRotateDelay);
        }

        [OnEventFire]
        public void StopRotate(HangarCameraDelayAutoRotateEvent e, Node any, [JoinAll] HangarCameraAutoRotateNode hangar) {
            hangar.Entity.RemoveComponent<HangarCameraAutoRotateComponent>();
        }

        public class HangarCameraTankViewStateNode : Node {
            public CameraComponent camera;
            public HangarCameraComponent hangarCamera;

            public HangarCameraTankViewComponent hangarCameraTankView;

            public HangarConfigComponent hangarConfig;

            public HangarTankPositionComponent hangarTankPosition;
        }

        public class HangarCameraTankViewStateRotationEnabledNode : HangarCameraTankViewStateNode {
            public HangarCameraRotationEnabledComponent hangarCameraRotationEnabled;
        }

        public class HangarCameraRotateScheduledTankViewStateNode : HangarCameraTankViewStateRotationEnabledNode {
            public HangarCameraRotateScheduledComponent hangarCameraRotateScheduled;
        }

        public class HangarCameraAutoRotateNode : HangarCameraRotateScheduledTankViewStateNode {
            public HangarCameraAutoRotateComponent hangarCameraAutoRotate;
        }
    }
}