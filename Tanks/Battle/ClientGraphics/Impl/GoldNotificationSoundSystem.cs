using Lobby.ClientSettings.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class GoldNotificationSoundSystem : ECSSystem {
        [OnEventFire]
        public void CreateAndPlayGoldNotificationSound(GoldScheduledNotificationEvent e, Node node,
            [JoinAll] NoGoldNotificationSoundListenerNode listener, [JoinAll] SelfBattleUserNode battleUser,
            [JoinAll] SingleNode<GoldSoundConfigComponent> config) =>
            listener.Entity.AddComponent<GoldNotificationPlaySoundComponent>();

        [OnEventFire]
        public void CleanGoldNotification(NodeRemoveEvent e, SingleNode<MapInstanceComponent> map,
            [JoinAll] SingleNode<GoldNotificationPlaySoundComponent> listener) =>
            listener.Entity.RemoveComponent<GoldNotificationPlaySoundComponent>();

        [OnEventFire]
        public void CreateAndPlayGoldNotificationSound(NodeAddedEvent evt, GoldNotificationSoundListenerNode listener,
            [JoinAll] SingleNode<MapInstanceComponent> map, [JoinAll] SingleNode<GoldSoundConfigComponent> config) {
            listener.Entity.RemoveComponent<GoldNotificationPlaySoundComponent>();
            GoldSoundConfigComponent component = config.component;
            AudioSource audioSource = Object.Instantiate(component.GoldNotificationSound);
            audioSource.transform.SetParent(map.component.SceneRoot.transform);
            float length = audioSource.clip.length;
            audioSource.Play();
            Object.Destroy(audioSource.gameObject, length);
        }

        public class SoundListenerNode : Node {
            public SoundListenerComponent soundListener;
        }

        public class GoldNotificationSoundListenerNode : SoundListenerNode {
            public GoldNotificationPlaySoundComponent goldNotificationPlaySound;

            public SoundListenerBattleStateComponent soundListenerBattleState;
        }

        [Not(typeof(GoldNotificationPlaySoundComponent))]
        public class NoGoldNotificationSoundListenerNode : SoundListenerNode { }

        public class SelfBattleUserNode : Node {
            public SelfBattleUserComponent selfBattleUser;

            public UserReadyToBattleComponent userReadyToBattle;
        }
    }
}