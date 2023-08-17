using System.Collections.Generic;
using System.Linq;
using Lobby.ClientEntrance.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientDataStructures.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using UnityEngine;
using Event = Platform.Kernel.ECS.ClientEntitySystem.API.Event;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class SaveLoadCameraSystem : ECSSystem {
        static readonly KeyCode[] saveKeys = new KeyCode[10] {
            KeyCode.Alpha1,
            KeyCode.Alpha2,
            KeyCode.Alpha3,
            KeyCode.Alpha4,
            KeyCode.Alpha5,
            KeyCode.Alpha6,
            KeyCode.Alpha7,
            KeyCode.Alpha8,
            KeyCode.Alpha9,
            KeyCode.Alpha0
        };

        [Inject] public static InputManager InputManager { get; set; }

        [OnEventComplete]
        public void CheckSaveOrLoadCamera(UpdateEvent evt, SpectatorCameraNode camera) {
            for (int i = 0; i < saveKeys.Length; i++) {
                if (InputManager.GetKeyDown(saveKeys[i])) {
                    camera.spectatorCamera.SaveCameraModificatorKeyHasBeenPressed = false;
                }

                if (InputManager.GetKey(saveKeys[i]) &&
                    InputManager.CheckAction(SpectatorCameraActions.SaveCameraModificator)) {
                    camera.spectatorCamera.SaveCameraModificatorKeyHasBeenPressed = true;
                }

                if (InputManager.GetKeyUp(saveKeys[i])) {
                    if (camera.spectatorCamera.SaveCameraModificatorKeyHasBeenPressed) {
                        ScheduleEvent(new SaveCameraEvent(i), camera.Entity);
                    } else {
                        ScheduleEvent(new LoadCameraEvent(i), camera.Entity);
                    }
                }
            }
        }

        [OnEventFire]
        public void Save(SaveCameraEvent e, FollowCameraNode camera,
            [JoinAll] SingleNode<FollowedBattleUserComponent> followedUser, [JoinByUser] UserNode user) {
            CameraSaveData value = CameraSaveData.CreateFollowData(user.userUid.Uid,
                camera.bezierPosition.BezierPosition.GetBaseRatio(),
                camera.bezierPosition.BezierPosition.GetRatioOffset());

            camera.spectatorCamera.savedCameras[e.slotIndex] = value;
        }

        [OnEventFire]
        public void Save(SaveCameraEvent e, MouseOrbitCameraNode camera,
            [JoinAll] SingleNode<FollowedBattleUserComponent> followedUser, [JoinByUser] UserNode user) {
            CameraSaveData value = CameraSaveData.CreateMouseOrbitData(user.userUid.Uid,
                camera.mouseOrbitCamera.distance,
                camera.mouseOrbitCamera.targetRotation);

            camera.spectatorCamera.savedCameras[e.slotIndex] = value;
        }

        [OnEventFire]
        public void Save(SaveCameraEvent e, FreeCameraNode camera) {
            CameraSaveData value = CameraSaveData.CreateFreeData(camera.camera.Camera.transform);
            camera.spectatorCamera.savedCameras[e.slotIndex] = value;
        }

        [OnEventFire]
        public void LoadCamera(LoadCameraEvent e, SpectatorCameraNode camera,
            [JoinAll] Optional<SingleNode<FollowedBattleUserComponent>> followedUser,
            [JoinAll] ICollection<UserNode> users) {
            if (!camera.spectatorCamera.savedCameras.ContainsKey(e.slotIndex)) {
                return;
            }

            CameraSaveData data = camera.spectatorCamera.savedCameras[e.slotIndex];

            if (data.Type == CameraType.Free) {
                if (followedUser.IsPresent()) {
                    followedUser.Get().Entity.RemoveComponent<FollowedBattleUserComponent>();
                }

                SetCameraLoading(camera, data);
                return;
            }

            UserNode userNode2 = users.ToList().SingleOrDefault(userNode => userNode.userUid.Uid == data.UserUid);

            if (userNode2 != null) {
                ScheduleEvent(new CameraLoadedSaveValidateEvent(data), userNode2.Entity);
            }
        }

        [OnEventFire]
        public void RemoveCurrentCameraController(CameraLoadedSaveValidateEvent e, UserNode user,
            [JoinByUser] SingleNode<UserInBattleAsTankComponent> userAsTank,
            [JoinByUser] SingleNode<BattleUserComponent> battleUser,
            [JoinAll] Optional<SingleNode<FollowedBattleUserComponent>> followedUser, [JoinAll] SpectatorCameraNode camera) {
            if (followedUser.IsPresent()) {
                followedUser.Get().Entity.RemoveComponent<FollowedBattleUserComponent>();
            }

            battleUser.Entity.AddComponent<FollowedBattleUserComponent>();
            SetCameraLoading(camera, e.SaveData);
        }

        void SetCameraLoading(SpectatorCameraNode camera, CameraSaveData data) {
            camera.transitionCamera.CameraSaveData = data;
            camera.cameraESM.esm.ChangeState<CameraStates.CameraTransitionState>();
        }

        public class CameraLoadedSaveValidateEvent : Event {
            public CameraLoadedSaveValidateEvent(CameraSaveData saveData) => SaveData = saveData;

            public CameraSaveData SaveData { get; set; }
        }

        public class SaveCameraEvent : Event {
            public int slotIndex;

            public SaveCameraEvent(int slotIndex) => this.slotIndex = slotIndex;
        }

        public class LoadCameraEvent : Event {
            public int slotIndex;

            public LoadCameraEvent(int slotIndex) => this.slotIndex = slotIndex;
        }

        [Not(typeof(TransitionCameraStateComponent))]
        public class SpectatorCameraNode : Node {
            public BezierPositionComponent bezierPosition;
            public CameraComponent camera;

            public CameraESMComponent cameraESM;

            public CameraTransformDataComponent cameraTransformData;

            public SpectatorCameraComponent spectatorCamera;

            public TransitionCameraComponent transitionCamera;
        }

        public class FollowCameraNode : SpectatorCameraNode {
            public FollowCameraComponent followCamera;
        }

        public class CameraTargetNode : Node {
            public CameraTargetComponent cameraTarget;

            public UserGroupComponent userGroup;

            public WeaponInstanceComponent weaponInstance;
        }

        public class UserNode : Node {
            public UserGroupComponent userGroup;

            public UserUidComponent userUid;
        }

        public class MouseOrbitCameraNode : SpectatorCameraNode {
            public MouseOrbitCameraComponent mouseOrbitCamera;
        }

        public class FreeCameraNode : SpectatorCameraNode {
            public FreeCameraComponent freeCamera;
        }

        public class WeaponNode : Node {
            public UserGroupComponent userGroup;

            public WeaponInstanceComponent weaponInstance;
        }
    }
}