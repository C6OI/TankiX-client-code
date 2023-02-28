using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientGraphics.API;
using Tanks.Lobby.ClientSettings.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class SoundListenerSystem : ECSSystem {
        [OnEventFire]
        public void InitSoundListenerInBattle(NodeAddedEvent evt, SingleNode<SoundListenerComponent> soundListener, [Context] [JoinAll] BattleCameraNode node) {
            ApplyListenerTransformToCamera(soundListener.component, node.cameraRootTransform.Root);
        }

        [OnEventFire]
        public void UpdateSoundListenerInBattle(UpdateEvent evt, SingleNode<SoundListenerComponent> soundListener, [JoinAll] BattleCameraNode node,
            [JoinAll] SingleNode<MapInstanceComponent> map) {
            ApplyListenerTransformToCamera(soundListener.component, node.cameraRootTransform.Root);
        }

        [OnEventFire]
        public void UpdateSoundListenerInHangar(UpdateEvent evt, SoundListenerInLobbyNode soundListener, [JoinAll] HangarCameraNode node) {
            ApplyListenerTransformToCamera(soundListener.soundListener, node.cameraRootTransform.Root);
        }

        void ApplyListenerTransformToCamera(SoundListenerComponent soundListener, Transform cameraTransform) {
            Transform transform = soundListener.transform;
            transform.position = cameraTransform.position;
            transform.rotation = cameraTransform.rotation;
        }

        public class CameraNode : Node {
            public CameraComponent camera;

            public CameraRootTransformComponent cameraRootTransform;
        }

        public class BattleCameraNode : CameraNode {
            public BattleCameraComponent battleCamera;
        }

        public class HangarCameraNode : CameraNode {
            public HangarCameraComponent hangarCamera;
        }

        public class SoundListenerInLobbyNode : Node {
            public SoundListenerComponent soundListener;

            public SoundListenerLobbyStateComponent soundListenerLobbyState;
        }
    }
}