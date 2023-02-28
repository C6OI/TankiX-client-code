using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientGraphics.Impl;
using Tanks.Lobby.ClientGarage.Impl;
using Tanks.Lobby.ClientNavigation.API;
using UnityEngine;

namespace Tanks.Lobby.ClientHangar.Impl {
    public class HangarCameraSwitchSystem : ECSSystem {
        [OnEventFire]
        public void InitHangarCamera(NodeAddedEvent e, HangarCameraInitNode hangar) {
            if ((bool)hangar.hangar) {
                Camera componentInChildren = hangar.hangar.GetComponentInChildren<Camera>();
                componentInChildren.transform.parent.position = hangar.hangarCameraStartPosition.transform.position;
                componentInChildren.transform.parent.LookAt(hangar.hangarTankPosition.transform.position);
                hangar.Entity.AddComponent(new CameraRootTransformComponent(componentInChildren.transform.parent));
                hangar.Entity.AddComponent<HangarCameraComponent>();
                hangar.Entity.AddComponent(new CameraComponent(componentInChildren));
                SetupCameraESM(hangar.Entity);
                SetupCameraViewESM(hangar.Entity);
                SetupCameraRotationESM(hangar.Entity);
            }
        }

        [OnEventFire]
        public void EnableHangarCameraRotation(NodeAddedEvent e, ScreenNode screen, HangarCameraRotationDisabledNode hangar) {
            if (screen.screen.RotateHangarCamera) {
                hangar.Entity.RemoveComponent<HangarCameraRotationDisabledComponent>();
                hangar.Entity.AddComponent<HangarCameraRotationEnabledComponent>();
            }
        }

        [OnEventFire]
        public void DisableHangarCameraRotation(NodeAddedEvent e, ScreenNode screen, HangarCameraRotationEnabledNode hangar) {
            if (!screen.screen.RotateHangarCamera) {
                hangar.Entity.RemoveComponent<HangarCameraRotationEnabledComponent>();
                hangar.Entity.AddComponent<HangarCameraRotationDisabledComponent>();
            }
        }

        [OnEventFire]
        public void EnableHangarCamera(NodeAddedEvent e, ScreenNode screen, HangarCameraDisabledNode hangar) {
            if (screen.screen.ShowHangar) {
                hangar.hangarCameraState.Esm.ChangeState<HangarCameraState.Enabled>();
            }
        }

        [OnEventFire]
        public void DisableHangarCamera(NodeAddedEvent e, ScreenNode screen, HangarCameraEnabledNode hangar) {
            if (!screen.screen.ShowHangar) {
                hangar.hangarCameraState.Esm.ChangeState<HangarCameraState.Disabled>();
            }
        }

        [OnEventFire]
        public void EnableHangarCamera(NodeAddedEvent e, HangarCameraEnabledNode hangar) {
            if (hangar.camera.Enabled) {
                hangar.camera.Enabled = true;
            }
        }

        [OnEventFire]
        public void DisableHangarCamera(NodeRemoveEvent e, HangarCameraEnabledNode hangar) {
            if (hangar.camera.Enabled) {
                hangar.camera.Enabled = false;
            }
        }

        [OnEventComplete]
        public void DeleteHangarCamera(NodeRemoveEvent e, SingleNode<HangarComponent> h, [JoinSelf] HangarCameraNode hangar) {
            hangar.Entity.RemoveComponent<HangarCameraViewStateComponent>();
            hangar.Entity.RemoveComponent<HangarCameraStateComponent>();
            hangar.Entity.RemoveComponent<HangarCameraRotationStateComponent>();
            hangar.Entity.RemoveComponent<HangarCameraComponent>();
            hangar.Entity.RemoveComponent<CameraComponent>();
            hangar.Entity.RemoveComponent<CameraRootTransformComponent>();
        }

        void SetupCameraESM(Entity camera) {
            HangarCameraStateComponent hangarCameraStateComponent = new();
            camera.AddComponent(hangarCameraStateComponent);
            EntityStateMachine esm = hangarCameraStateComponent.Esm;
            esm.AddState<HangarCameraState.Enabled>();
            esm.AddState<HangarCameraState.Disabled>();
            esm.ChangeState<HangarCameraState.Disabled>();
        }

        void SetupCameraViewESM(Entity camera) {
            HangarCameraViewStateComponent hangarCameraViewStateComponent = new();
            camera.AddComponent(hangarCameraViewStateComponent);
            EntityStateMachine esm = hangarCameraViewStateComponent.Esm;
            esm.AddState<HangarCameraViewState.TankViewState>();
            esm.AddState<HangarCameraViewState.FlightToLocationState>();
            esm.AddState<HangarCameraViewState.LocationViewState>();
            esm.AddState<HangarCameraViewState.FlightToTankState>();
            esm.ChangeState<HangarCameraViewState.TankViewState>();
        }

        void SetupCameraRotationESM(Entity camera) {
            HangarCameraRotationStateComponent hangarCameraRotationStateComponent = new();
            camera.AddComponent(hangarCameraRotationStateComponent);
            EntityStateMachine esm = hangarCameraRotationStateComponent.Esm;
            esm.AddState<HangarCameraRotationState.Enabled>();
            esm.AddState<HangarCameraRotationState.Disabled>();
            esm.ChangeState<HangarCameraRotationState.Disabled>();
        }

        [OnEventFire]
        public void OnMain(NodeAddedEvent e, SingleNode<MainScreenComponent> screen, HangarCameraNode camera) {
            camera.hangar.GetComponentInChildren<Camera>().GetComponent<CameraOffsetBehaviour>().SetOffset(0f);
        }

        public class ScreenNode : Node {
            public ActiveScreenComponent activeScreen;
            public ScreenComponent screen;
        }

        public class HangarCameraInitNode : Node {
            public HangarComponent hangar;

            public HangarCameraStartPositionComponent hangarCameraStartPosition;

            public HangarTankPositionComponent hangarTankPosition;
        }

        public class HangarCameraNode : Node {
            public CameraComponent camera;
            public HangarComponent hangar;

            public HangarCameraComponent hangarCamera;

            public HangarCameraRotationStateComponent hangarCameraRotationState;

            public HangarCameraStateComponent hangarCameraState;

            public HangarCameraViewStateComponent hangarCameraViewState;
        }

        public class HangarCameraEnabledNode : HangarCameraNode {
            public HangarCameraEnabledComponent hangarCameraEnabled;
        }

        public class HangarCameraDisabledNode : HangarCameraNode {
            public HangarCameraDisabledComponent hangarCameraDisabled;
        }

        public class HangarCameraRotationEnabledNode : HangarCameraNode {
            public HangarCameraRotationEnabledComponent hangarCameraRotationEnabled;
        }

        public class HangarCameraRotationDisabledNode : HangarCameraNode {
            public HangarCameraRotationDisabledComponent hangarCameraRotationDisabled;
        }
    }
}