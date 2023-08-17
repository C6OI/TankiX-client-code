using Lobby.ClientSettings.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class SoundListenerSystem : ECSSystem {
        [OnEventFire]
        public void InitSoundListenerInBattle(NodeAddedEvent evt, SingleNode<SoundListenerComponent> soundListener,
            [Context] [JoinAll] CameraNode node) => ApplyListenerTransformToCamera(soundListener, node);

        [OnEventFire]
        public void UpdateSoundListenerInBattle(UpdateEvent evt, SingleNode<SoundListenerComponent> soundListener,
            [JoinAll] CameraNode node, [JoinAll] SingleNode<MapInstanceComponent> map) =>
            ApplyListenerTransformToCamera(soundListener, node);

        void ApplyListenerTransformToCamera(SingleNode<SoundListenerComponent> soundListener, CameraNode node) {
            Transform transform = soundListener.component.transform;
            Transform transform2 = node.camera.Camera.transform;
            transform.position = transform2.position;
            transform.rotation = transform2.rotation;
        }

        public class CameraNode : Node {
            public BattleCameraComponent battleCamera;

            public CameraComponent camera;
        }
    }
}