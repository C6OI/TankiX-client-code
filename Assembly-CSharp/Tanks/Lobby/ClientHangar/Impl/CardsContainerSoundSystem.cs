using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;
using Tanks.Battle.ClientGraphics.API;
using Tanks.Lobby.ClientGarage.Impl;
using Tanks.Lobby.ClientSettings.API;
using UnityEngine;
using UnityEngine.Audio;

namespace Tanks.Lobby.ClientHangar.Impl {
    public class CardsContainerSoundSystem : ECSSystem {
        [OnEventFire]
        public void PlayOpenSound(OpenVisualContainerEvent e, Node any, [JoinAll] SingleNode<CardsContainerSoundsComponent> container, [JoinAll] SoundListenerNode listener) {
            Transform transform = listener.soundListener.transform;
            CleanUpAllOpenCardsContainerSound(transform);
            CardsSoundBehaviour cardsSoundBehaviour = Object.Instantiate(container.component.CardsSounds);
            cardsSoundBehaviour.transform.SetParent(transform);
            cardsSoundBehaviour.transform.localRotation = Quaternion.identity;
            cardsSoundBehaviour.transform.localPosition = Vector3.zero;

            SwitchMusicMixerToSnapshot(listener.soundListenerResources.Resources.MusicMixerSnapshots[listener.soundListenerMusicSnapshots.CardsContainerMusicSnapshot],
                listener.soundListenerMusicTransitions.TransitionCardsContainerTheme, listener);

            cardsSoundBehaviour.OpenCardsContainerSource.FadeIn();
            AudioClip clip = cardsSoundBehaviour.OpenCardsContainerSource.Source.clip;
            Object.DestroyObject(cardsSoundBehaviour.gameObject, clip.length + 0.5f);
            listener.Entity.RemoveComponentIfPresent<CardsContainerOpeningSoundWaitForFinishComponent>();
            listener.Entity.RemoveComponentIfPresent<ModuleGarageSoundWaitForFinishComponent>();

            listener.Entity.AddComponent(
                new CardsContainerOpeningSoundWaitForFinishComponent(NewEvent<CardsContainerOpeningSoundFinishEvent>().Attach(listener).ScheduleDelayed(clip.length).Manager()));
        }

        [OnEventFire]
        public void Cancel(NodeRemoveEvent e, SingleNode<CardsContainerOpeningSoundWaitForFinishComponent> node) {
            node.component.ScheduledEvent.Cancel();
        }

        [OnEventFire]
        public void SwitchSnapshot(CardsContainerOpeningSoundFinishEvent e, SoundListenerNode listener) {
            SwitchMusicMixerToSnapshot(listener.soundListenerResources.Resources.MusicMixerSnapshots[listener.soundListenerMusicSnapshots.HymnLoopSnapshot],
                listener.soundListenerMusicTransitions.TransitionCardsContainerTheme, listener);
        }

        [OnEventComplete]
        public void Cancel(CardsContainerOpeningSoundFinishEvent e, SingleNode<CardsContainerOpeningSoundWaitForFinishComponent> node) {
            node.Entity.RemoveComponent<CardsContainerOpeningSoundWaitForFinishComponent>();
        }

        [OnEventFire]
        public void CleanUpAllOpenCardsContainerSound(MapAmbientSoundPlayEvent e, SoundListenerNode listener) {
            CleanUpAllOpenCardsContainerSound(listener.soundListener.transform);
        }

        void CleanUpAllOpenCardsContainerSound(Transform listenerTransform) {
            CardsSoundBehaviour[] componentsInChildren = listenerTransform.gameObject.GetComponentsInChildren<CardsSoundBehaviour>();

            componentsInChildren.ForEach(delegate(CardsSoundBehaviour s) {
                s.OpenCardsContainerSource.FadeOut();
                Object.DestroyObject(s.gameObject, s.OpenCardsContainerSource.FadeOutTimeSec + 0.5f);
            });
        }

        void SwitchMusicMixerToSnapshot(AudioMixerSnapshot snapshot, float transition, SoundListenerNode listener) {
            listener.soundListenerResources.Resources.MusicMixer.TransitionToSnapshots(new AudioMixerSnapshot[1] { snapshot }, new float[1] { 1f }, transition);
        }

        public class SoundListenerNode : Node {
            public SoundListenerComponent soundListener;

            public SoundListenerMusicSnapshotsComponent soundListenerMusicSnapshots;

            public SoundListenerMusicTransitionsComponent soundListenerMusicTransitions;

            public SoundListenerResourcesComponent soundListenerResources;
        }
    }
}