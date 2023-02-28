using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;
using Tanks.Battle.ClientGraphics.API;
using Tanks.Battle.ClientGraphics.Impl;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientGarage.Impl;
using Tanks.Lobby.ClientSettings.API;
using UnityEngine;
using UnityEngine.Audio;

namespace Tanks.Lobby.ClientHangar.Impl {
    public class HangarModuleSoundsSystem : ECSSystem {
        [OnEventFire]
        public void PlayModuleActivationSound(ModuleAssembledEvent e, SingleNode<UserComponent> node, [JoinAll] SoundListenerNode listener) {
            PlayModuleSound(listener.soundListenerResources.Resources.ModuleActivation, listener);
        }

        [OnEventFire]
        public void PlayModuleUpgradeSound(ModuleUpgradedEvent e, SingleNode<UserItemComponent> node, [JoinAll] SoundListenerNode listener) {
            PlayModuleSound(listener.soundListenerResources.Resources.ModuleUpgrade, listener);
        }

        [OnEventFire]
        public void Cancel(NodeRemoveEvent e, SingleNode<ModuleGarageSoundWaitForFinishComponent> node) {
            node.component.ScheduledEvent.Cancel();
        }

        [OnEventFire]
        public void SwitchSnapshot(ModuleGarageSoundFinishEvent e, SoundListenerNode listener) {
            SwitchMusicMixerToSnapshot(listener.soundListenerResources.Resources.MusicMixerSnapshots[listener.soundListenerMusicSnapshots.HymnLoopSnapshot],
                listener.soundListenerMusicTransitions.TransitionModuleTheme, listener);
        }

        [OnEventComplete]
        public void Cancel(ModuleGarageSoundFinishEvent e, SingleNode<ModuleGarageSoundWaitForFinishComponent> node) {
            node.Entity.RemoveComponent<ModuleGarageSoundWaitForFinishComponent>();
        }

        [OnEventFire]
        public void CleanUp(MapAmbientSoundPlayEvent e, SoundListenerNode listener) {
            CleanUpAllGarageModuleSound(listener.soundListener.transform);
        }

        void PlayModuleSound(GameObject source, SoundListenerNode listener) {
            Transform transform = listener.soundListener.transform;
            CleanUpAllGarageModuleSound(transform);
            SoundController component = Object.Instantiate(source, transform.position, transform.rotation, transform).GetComponent<SoundController>();

            SwitchMusicMixerToSnapshot(listener.soundListenerResources.Resources.MusicMixerSnapshots[listener.soundListenerMusicSnapshots.GarageModuleMusicSnapshot],
                listener.soundListenerMusicTransitions.TransitionModuleTheme, listener);

            component.SetSoundActive();
            Object.DestroyObject(component.gameObject, component.Source.clip.length + 0.1f);
            listener.Entity.RemoveComponentIfPresent<CardsContainerOpeningSoundWaitForFinishComponent>();
            listener.Entity.RemoveComponentIfPresent<ModuleGarageSoundWaitForFinishComponent>();

            listener.Entity.AddComponent(
                new ModuleGarageSoundWaitForFinishComponent(NewEvent<ModuleGarageSoundFinishEvent>().Attach(listener).ScheduleDelayed(component.Source.clip.length).Manager()));
        }

        void CleanUpAllGarageModuleSound(Transform listenerTransform) {
            HangarModuleSoundBehaviour[] componentsInChildren = listenerTransform.gameObject.GetComponentsInChildren<HangarModuleSoundBehaviour>();

            componentsInChildren.ForEach(delegate(HangarModuleSoundBehaviour s) {
                SoundController component = s.GetComponent<SoundController>();
                component.FadeOut();
                Object.DestroyObject(s.gameObject, component.FadeOutTimeSec + 0.2f);
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