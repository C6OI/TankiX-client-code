using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientGraphics.API;
using Tanks.Lobby.ClientBattleSelect.API;
using Tanks.Lobby.ClientBattleSelect.Impl;
using Tanks.Lobby.ClientGarage.Impl;
using Tanks.Lobby.ClientLoading.API;
using Tanks.Lobby.ClientNavigation.API;
using Tanks.Lobby.ClientSettings.API;
using UnityEngine;
using UnityEngine.Audio;

namespace Tanks.Lobby.ClientHangar.Impl {
    public class HangarAmbientSoundSystem : ECSSystem {
        [OnEventFire]
        public void PrepareAmbientSoundEffect(NodeAddedEvent evt, InitialHangarAmbientSoundNode hangar, NotHangarAmbientSoundListenerNode soundListener) {
            Entity entity = soundListener.Entity;
            entity.AddComponent(new HangarAmbientSoundControllerComponent(PrepareNewEffect(hangar, soundListener)));
            entity.AddComponent<HangarAmbientSoundSilenceComponent>();
        }

        HangarAmbientSoundController PrepareNewEffect(InitialHangarAmbientSoundNode hangar, SoundListenerNode soundListener) {
            HangarAmbientSoundPrefabComponent hangarAmbientSoundPrefab = hangar.hangarAmbientSoundPrefab;
            HangarAmbientSoundController hangarAmbientSoundController = hangarAmbientSoundPrefab.HangarAmbientSoundController;
            HangarAmbientSoundController hangarAmbientSoundController2 = Object.Instantiate(hangarAmbientSoundController);
            Transform transform = hangarAmbientSoundController2.transform;
            transform.parent = soundListener.soundListener.transform;
            transform.localRotation = Quaternion.identity;
            transform.localPosition = Vector3.zero;
            return hangarAmbientSoundController2;
        }

        [OnEventFire]
        public void PlayAmbientSoundEffect(NodeAddedEvent evt, ReadySilentAmbientSoundNode soundListener, HomeScreenNode screen) {
            NewEvent(new BeforeLobbyAmbientSoundPlayEvent(true)).Attach(soundListener).ScheduleDelayed(soundListener.soundListener.DelayForLobbyState);
        }

        [OnEventFire]
        public void PlayAmbientSoundEffect(NodeAddedEvent evt, ReadySilentAmbientSoundNode soundListener, BattleResultsScreenNode screen) {
            NewEvent(new BeforeLobbyAmbientSoundPlayEvent(false)).Attach(soundListener).ScheduleDelayed(soundListener.soundListener.DelayForLobbyState);
        }

        [OnEventFire]
        public void PlayAmbientSoundEffect(NodeAddedEvent evt, ReadySilentAmbientSoundNode soundListener, BattleSelectScreenNode screen) {
            NewEvent(new BeforeLobbyAmbientSoundPlayEvent(true)).Attach(soundListener).ScheduleDelayed(soundListener.soundListener.DelayForLobbyState);
        }

        [OnEventFire]
        public void FilterLobbyAmbientSoundPlayEvent(BeforeLobbyAmbientSoundPlayEvent evt, SoundListenerNode listener, [JoinAll] SingleNode<HangarInstanceComponent> hangar,
            [JoinAll] PlayLobbySoundScreenNode screen, [JoinAll] Optional<SingleNode<MapInstanceComponent>> map) {
            if (!map.IsPresent()) {
                ScheduleEvent(new LobbyAmbientSoundPlayEvent(evt.HymnMode), listener);
            }
        }

        [OnEventFire]
        public void SetBattleResultSnapshot(LobbyAmbientSoundPlayEvent evt, ReadySilentPlayedAmbientSoundNode soundListener) {
            SwitchMusicMixerToSnapshot(
                soundListener.soundListenerResources.Resources.MusicMixerSnapshots[
                    !evt.HymnMode ? soundListener.soundListenerMusicSnapshots.BattleResultMusicSnapshot : soundListener.soundListenerMusicSnapshots.HymnLoopSnapshot], 0f, soundListener);
        }

        [OnEventFire]
        public void SwitchToHymmSnapshot(NodeAddedEvent evt, HomeScreenNode screen, [JoinAll] ReadyNonSilentAmbientSoundNode listener) {
            SwitchMusicMixerToSnapshot(listener.soundListenerResources.Resources.MusicMixerSnapshots[listener.soundListenerMusicSnapshots.HymnLoopSnapshot],
                listener.soundListenerMusicTransitions.MusicTransitionSec, listener);
        }

        void SwitchMusicMixerToSnapshot(AudioMixerSnapshot snapshot, float transition, SoundListenerNode listener) {
            listener.soundListenerResources.Resources.MusicMixer.TransitionToSnapshots(new AudioMixerSnapshot[1] { snapshot }, new float[1] { 1f }, transition);
        }

        [OnEventComplete]
        public void PlayAmbientSoundEffect(LobbyAmbientSoundPlayEvent evt, ReadySilentPlayedAmbientSoundNode soundListener) {
            Play(soundListener, false);
        }

        [OnEventComplete]
        public void PlayAmbientSoundEffect(LobbyAmbientSoundPlayEvent evt, ReadySilentNotPlayedAmbientSoundNode soundListener) {
            soundListener.Entity.AddComponent<HangarAmbientSoundAlreadyPlayedComponent>();
            Play(soundListener, true);
        }

        [OnEventFire]
        public void FinalizeAmbientSoundEffect(NodeAddedEvent evt, BattleScreenNode battleScreen, [JoinAll] SoundListenerNode soundListener) {
            NewEvent<BeforeMapAmbientSoundPlayEvent>().Attach(soundListener).ScheduleDelayed(soundListener.soundListener.DelayForBattleEnterState);
        }

        [OnEventFire]
        public void FilterMapAmbientSoundPlayEvent(BeforeMapAmbientSoundPlayEvent evt, SoundListenerNode soundListener, [JoinAll] SingleNode<MapInstanceComponent> map,
            [JoinAll] BattleScreenNode battleScreen) {
            ScheduleEvent<MapAmbientSoundPlayEvent>(soundListener);
        }

        [OnEventFire]
        public void FinalizeAmbientSoundEffect(MapAmbientSoundPlayEvent evt, ReadyNonSilentAmbientSoundNode soundListener) {
            Stop(soundListener);
        }

        [OnEventFire]
        public void FinalizeAmbientSoundEffect(MapAmbientSoundPlayEvent evt, ReadySilentAmbientSoundNode soundListener) {
            Stop(soundListener);
            soundListener.Entity.RemoveComponent<HangarAmbientSoundSilenceComponent>();
        }

        void Stop(HangarAmbientSoundListenerNode soundListener) {
            HangarAmbientSoundControllerComponent hangarAmbientSoundController = soundListener.hangarAmbientSoundController;
            HangarAmbientSoundController hangarAmbientSoundController2 = hangarAmbientSoundController.HangarAmbientSoundController;
            soundListener.Entity.RemoveComponent<HangarAmbientSoundControllerComponent>();
            hangarAmbientSoundController2.Stop();
        }

        void Play(ReadySilentAmbientSoundNode soundListener, bool playWithNitro) {
            Entity entity = soundListener.Entity;
            HangarAmbientSoundControllerComponent hangarAmbientSoundController = soundListener.hangarAmbientSoundController;
            HangarAmbientSoundController hangarAmbientSoundController2 = hangarAmbientSoundController.HangarAmbientSoundController;
            hangarAmbientSoundController2.Play(playWithNitro);
            entity.RemoveComponent<HangarAmbientSoundSilenceComponent>();
        }

        public class InitialHangarAmbientSoundNode : Node {
            public HangarAmbientSoundPrefabComponent hangarAmbientSoundPrefab;
        }

        public class SoundListenerNode : Node {
            public SoundListenerComponent soundListener;

            public SoundListenerMusicSnapshotsComponent soundListenerMusicSnapshots;

            public SoundListenerMusicTransitionsComponent soundListenerMusicTransitions;

            public SoundListenerResourcesComponent soundListenerResources;
        }

        [Not(typeof(HangarAmbientSoundControllerComponent))]
        [Not(typeof(HangarAmbientSoundSilenceComponent))]
        public class NotHangarAmbientSoundListenerNode : SoundListenerNode { }

        public class HangarAmbientSoundListenerNode : SoundListenerNode {
            public HangarAmbientSoundControllerComponent hangarAmbientSoundController;
        }

        [Not(typeof(HangarAmbientSoundSilenceComponent))]
        public class ReadyNonSilentAmbientSoundNode : HangarAmbientSoundListenerNode { }

        public class ReadySilentAmbientSoundNode : HangarAmbientSoundListenerNode {
            public HangarAmbientSoundSilenceComponent hangarAmbientSoundSilence;
        }

        public class ReadySilentPlayedAmbientSoundNode : ReadySilentAmbientSoundNode {
            public HangarAmbientSoundAlreadyPlayedComponent hangarAmbientSoundAlreadyPlayed;
        }

        [Not(typeof(HangarAmbientSoundAlreadyPlayedComponent))]
        public class ReadySilentNotPlayedAmbientSoundNode : ReadySilentAmbientSoundNode { }

        public class BattleResultsScreenNode : Node {
            public ActiveScreenComponent activeScreen;
            public BattleResultScreenComponent battleResultScreen;
        }

        public class HomeScreenNode : Node {
            public ActiveScreenComponent activeScreen;
            public MainScreenComponent mainScreen;
        }

        public class BattleScreenNode : Node {
            public ActiveScreenComponent activeScreen;
            public BattleScreenComponent battleScreen;
        }

        public class BattleSelectScreenNode : Node {
            public ActiveScreenComponent activeScreen;

            public BattleSelectScreenComponent battleSelectScreen;
        }

        [Not(typeof(BattleLoadScreenComponent))]
        [Not(typeof(BattleScreenComponent))]
        public class PlayLobbySoundScreenNode : Node {
            public ActiveScreenComponent activeScreen;

            public ScreenComponent screen;
        }
    }
}