using Lobby.ClientEntrance.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class BattleCameraBuilderSystem : ECSSystem {
        const string MAP_CONFIG_PATH = "camera";

        [OnEventFire]
        public void CreateBattleCamera(NodeAddedEvent evt, SingleNode<MapInstanceComponent> node) {
            GameObject gameObject = GameObject.Find(ClientGraphicsConstants.MAIN_CAMERA_NAME);
            Entity entity = CreateEntity(typeof(CameraTemplate), "camera");
            gameObject.GetComponent<EntityBehaviour>().BuildEntity(entity);
            entity.AddComponent<BattleCameraComponent>();
            CameraComponent cameraComponent = new();
            Camera component = gameObject.GetComponent<Camera>();
            Transform transform = component.transform;
            cameraComponent.Camera = component;
            entity.AddComponent(cameraComponent);
            CameraTransformDataComponent cameraTransformDataComponent = new();

            cameraTransformDataComponent.Data = new TransformData {
                Position = transform.position,
                Rotation = transform.rotation
            };

            entity.AddComponent(cameraTransformDataComponent);
            entity.AddComponent<CameraFOVUpdateComponent>();
            entity.AddComponent<BezierPositionComponent>();
            entity.AddComponent<ApplyCameraTransformComponent>();
            entity.AddComponent<TransitionCameraComponent>();
            BurningTargetBloomComponent burningTargetBloomComponent = new();
            burningTargetBloomComponent.burningTargetBloom = component.GetComponent<BurningTargetBloom>();
            entity.AddComponent(burningTargetBloomComponent);
            SetupCameraESM(entity);
        }

        [OnEventFire]
        public void DeleteCamera(NodeRemoveEvent evt, SingleNode<MapInstanceComponent> node,
            [JoinAll] SingleNode<BattleCameraComponent> camera) => DeleteEntity(camera.Entity);

        [OnEventComplete]
        public void SetTankAsReadyForCameraJoining(TankMovementInitEvent evt, SelfTankNode tank) =>
            tank.Entity.AddComponent<SelfTankReadyForCameraComponent>();

        [OnEventComplete]
        public void SetTankAsReadyForCameraJoining(TankMovementInitEvent evt, RemoteTankNode tank,
            [JoinByUser] FollowedBattleUserNode followedBattleUser) =>
            tank.Entity.AddComponent<FollowedTankReadyToCameraComponent>();

        [OnEventFire]
        public void FollowNewUser(NodeAddedEvent e, WeaponNode weapon,
            [JoinByUser] FollowedBattleUserNode followedBattleUser) {
            CameraTargetComponent component = new(weapon.weaponInstance.WeaponInstance);
            weapon.Entity.AddComponent(component);
        }

        [OnEventFire]
        public void AddCameraTarget(NodeAddedEvent e, WeaponNode weapon,
            [JoinByTank] [Context] SelfTankReadyForCameraNode tank) {
            CameraTargetComponent cameraTargetComponent = new();
            cameraTargetComponent.TargetObject = weapon.weaponInstance.WeaponInstance.gameObject;
            weapon.Entity.AddComponent(cameraTargetComponent);
        }

        [OnEventFire]
        public void SwitchCameraToSpawnState(NodeAddedEvent evt, SingleNode<FollowedTankReadyToCameraComponent> tank,
            [JoinByUser] SingleNode<UserUidComponent> userUidNode, [JoinAll] FollowESMNode camera) {
            TransitionCameraComponent transitionCamera = camera.transitionCamera;

            transitionCamera.CameraSaveData = CameraSaveData.CreateFollowData(userUidNode.component.Uid,
                camera.bezierPosition.BezierPosition.GetBaseRatio(),
                camera.bezierPosition.BezierPosition.GetRatioOffset());

            transitionCamera.Spawn = true;
            camera.cameraESM.Esm.ChangeState<CameraStates.CameraTransitionState>();
        }

        [OnEventFire]
        public void SwitchCameraToSpawnState(NodeAddedEvent evt, SingleNode<FollowedTankReadyToCameraComponent> tank,
            [JoinByUser] SingleNode<UserUidComponent> userUidNode, [JoinAll] MouseOrbitESMNode camera) {
            TransitionCameraComponent transitionCamera = camera.transitionCamera;

            transitionCamera.CameraSaveData = CameraSaveData.CreateMouseOrbitData(userUidNode.component.Uid,
                camera.mouseOrbitCamera.distance,
                camera.mouseOrbitCamera.targetRotation);

            transitionCamera.Spawn = true;
            camera.cameraESM.Esm.ChangeState<CameraStates.CameraTransitionState>();
        }

        [OnEventFire]
        public void SwitchCameraToSpawnState(NodeAddedEvent evt, SelfTankReadyForCameraNode tank,
            [JoinByUser] SingleNode<UserUidComponent> userUidNode, [Context] [JoinAll] ESMNode camera,
            [JoinAll] Optional<SingleNode<FollowCameraComponent>> followCameraOptional) {
            TransitionCameraComponent transitionCamera = camera.transitionCamera;

            transitionCamera.CameraSaveData = CameraSaveData.CreateFollowData(userUidNode.component.Uid,
                camera.bezierPosition.BezierPosition.GetBaseRatio(),
                camera.bezierPosition.BezierPosition.GetRatioOffset());

            transitionCamera.Spawn = true;
            camera.cameraESM.Esm.ChangeState<CameraStates.CameraTransitionState>();
        }

        void SetupCameraESM(Entity cameraEntity) {
            CameraESMComponent cameraESMComponent = new();
            cameraEntity.AddComponent(cameraESMComponent);
            EntityStateMachine esm = cameraESMComponent.Esm;
            esm.AddState<CameraStates.CameraFollowState>();
            esm.AddState<CameraStates.CameraFreeState>();
            esm.AddState<CameraStates.CameraGoState>();
            esm.AddState<CameraStates.CameraOrbitState>();
            esm.AddState<CameraStates.CameraTransitionState>();
        }

        [OnEventFire]
        public void ResetTransitionCamera(NodeRemoveEvent e, CameraNode camera) => camera.transitionCamera.Reset();

        public class ESMNode : Node {
            public BezierPositionComponent bezierPosition;
            public CameraESMComponent cameraESM;

            public TransitionCameraComponent transitionCamera;
        }

        public class FollowESMNode : ESMNode {
            public FollowCameraComponent followCamera;
        }

        public class MouseOrbitESMNode : ESMNode {
            public MouseOrbitCameraComponent mouseOrbitCamera;
        }

        public class SelfTankNode : Node {
            public SelfTankComponent selfTank;
            public TankGroupComponent tankGroup;
        }

        public class RemoteTankNode : Node {
            public RemoteTankComponent remoteTank;
            public UserGroupComponent userGroup;
        }

        public class FollowedBattleUserNode : Node {
            public FollowedBattleUserComponent followedBattleUser;

            public UserGroupComponent userGroup;
        }

        public class SelfTankReadyForCameraNode : Node {
            public SelfTankComponent selfTank;

            public SelfTankReadyForCameraComponent selfTankReadyForCamera;
            public TankGroupComponent tankGroup;
        }

        public class WeaponNode : Node {
            public TankGroupComponent tankGroup;

            public WeaponInstanceComponent weaponInstance;
        }

        public class CameraNode : Node {
            public TransitionCameraComponent transitionCamera;

            public TransitionCameraStateComponent transitionCameraState;
        }
    }
}