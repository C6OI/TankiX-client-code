using Lobby.ClientNavigation.API;
using Lobby.ClientSettings.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientGraphics.API;
using Tanks.Battle.ClientHUD.API;
using Tanks.Lobby.ClientBattleSelect.API;
using UnityEngine;

namespace Tanks.Lobby.ClientHangar.Impl {
    public class HangarAmbientSoundSystem : ECSSystem {
        [OnEventFire]
        public void PrepareAmbientSoundEffect(NodeAddedEvent evt, InitialHangarAmbientSoundNode hangar,
            NotHangarAmbientSoundListenerNode soundListener) {
            Entity entity = soundListener.Entity;
            entity.AddComponent(new HangarAmbientSoundControllerComponent(PrepareNewEffect(hangar, soundListener)));
            entity.AddComponent<HangarAmbientSoundSilenceComponent>();
        }

        HangarAmbientSoundController
            PrepareNewEffect(InitialHangarAmbientSoundNode hangar, SoundListenerNode soundListener) {
            HangarAmbientSoundPrefabComponent hangarAmbientSoundPrefab = hangar.hangarAmbientSoundPrefab;

            HangarAmbientSoundController hangarAmbientSoundController =
                hangarAmbientSoundPrefab.HangarAmbientSoundController;

            HangarAmbientSoundController hangarAmbientSoundController2 = Object.Instantiate(hangarAmbientSoundController);
            Transform transform = hangarAmbientSoundController2.transform;
            transform.parent = soundListener.soundListener.transform;
            transform.localRotation = Quaternion.identity;
            transform.localPosition = Vector3.zero;
            return hangarAmbientSoundController2;
        }

        [OnEventFire]
        public void PlayAmbientSoundEffect(NodeAddedEvent evt, ReadySilentAmbientSoundNode soundListener,
            HomeScreenNode screen) => NewEvent<LobbyAmbientSoundPlayEvent>().Attach(soundListener)
            .ScheduleDelayed(soundListener.soundListener.DelayForLobbyState);

        [OnEventFire]
        public void PlayAmbientSoundEffect(NodeAddedEvent evt, ReadySilentAmbientSoundNode soundListener,
            BattleSelectScreenNode screen) => NewEvent<LobbyAmbientSoundPlayEvent>().Attach(soundListener)
            .ScheduleDelayed(soundListener.soundListener.DelayForLobbyState);

        [OnEventFire]
        public void PlayAmbientSoundEffect(LobbyAmbientSoundPlayEvent evt,
            ReadySilentPlayedAmbientSoundNode soundListener) => Play(soundListener, false);

        [OnEventFire]
        public void PlayAmbientSoundEffect(LobbyAmbientSoundPlayEvent evt,
            ReadySilentNotPlayedAmbientSoundNode soundListener) {
            soundListener.Entity.AddComponent<HangarAmbientSoundAlreadyPlayedComponent>();
            Play(soundListener, true);
        }

        [OnEventFire]
        public void FinalizeAmbientSoundEffect(NodeAddedEvent evt, SingleNode<BattleScreenComponent> battleScreen,
            [JoinAll] SoundListenerNode soundListener) => NewEvent<MapAmbientSoundPlayEvent>().Attach(soundListener)
            .ScheduleDelayed(soundListener.soundListener.DelayForBattleEnterState);

        [OnEventFire]
        public void FinalizeAmbientSoundEffect(MapAmbientSoundPlayEvent evt, ReadyNonSilentAmbientSoundNode soundListener) =>
            Stop(soundListener);

        [OnEventFire]
        public void FinalizeAmbientSoundEffect(MapAmbientSoundPlayEvent evt, ReadySilentAmbientSoundNode soundListener) {
            Stop(soundListener);
            soundListener.Entity.RemoveComponent<HangarAmbientSoundSilenceComponent>();
        }

        void Stop(HangarAmbientSoundListenerNode soundListener) {
            HangarAmbientSoundControllerComponent hangarAmbientSoundController = soundListener.hangarAmbientSoundController;

            HangarAmbientSoundController hangarAmbientSoundController2 =
                hangarAmbientSoundController.HangarAmbientSoundController;

            soundListener.Entity.RemoveComponent<HangarAmbientSoundControllerComponent>();
            hangarAmbientSoundController2.Stop();
        }

        void Play(ReadySilentAmbientSoundNode soundListener, bool playWithNitro) {
            Entity entity = soundListener.Entity;
            HangarAmbientSoundControllerComponent hangarAmbientSoundController = soundListener.hangarAmbientSoundController;

            HangarAmbientSoundController hangarAmbientSoundController2 =
                hangarAmbientSoundController.HangarAmbientSoundController;

            hangarAmbientSoundController2.Play(playWithNitro);
            entity.RemoveComponent<HangarAmbientSoundSilenceComponent>();
        }

        public class InitialHangarAmbientSoundNode : Node {
            public HangarAmbientSoundPrefabComponent hangarAmbientSoundPrefab;
        }

        public class SoundListenerNode : Node {
            public SoundListenerComponent soundListener;
        }

        [Not(typeof(HangarAmbientSoundSilenceComponent))]
        [Not(typeof(HangarAmbientSoundControllerComponent))]
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

        public class HomeScreenNode : Node {
            public ActiveScreenComponent activeScreen;
            public HomeScreenComponent homeScreen;
        }

        public class BattleSelectScreenNode : Node {
            public ActiveScreenComponent activeScreen;

            public BattleSelectScreenComponent battleSelectScreen;
        }
    }
}