using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientGraphics.API;
using Tanks.Lobby.ClientGarage.Impl;
using Tanks.Lobby.ClientSettings.API;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class SoundListenerStateSystem : ECSSystem {
        [OnEventFire]
        public void InitSoundListenerESM(NodeAddedEvent evt, SoundListenerNode listener) {
            SoundListenerESMComponent soundListenerESMComponent = new();
            EntityStateMachine esm = soundListenerESMComponent.Esm;
            esm.AddState<SoundListenerStates.SoundListenerSpawnState>();
            esm.AddState<SoundListenerStates.SoundListenerBattleState>();
            esm.AddState<SoundListenerStates.SoundListenerLobbyState>();
            esm.AddState<SoundListenerStates.SoundListenerBattleFinishState>();
            esm.AddState<SoundListenerStates.SoundListenerSelfRankRewardState>();
            listener.Entity.AddComponent(soundListenerESMComponent);
        }

        [OnEventFire]
        public void SwitchToSelfRankRewardState(NodeAddedEvent e, SingleNode<SelfUserRankSoundEffectInstanceComponent> effect, [JoinAll] SoundListenerNotBattleFinishStateNode listener,
            [JoinAll] SingleNode<MapInstanceComponent> map, [JoinAll] SingleNode<RoundActiveStateComponent> round) {
            ScheduleEvent<SwitchSoundListenerStateEvent<SoundListenerStates.SoundListenerSelfRankRewardState>>(listener);
        }

        [OnEventFire]
        public void SwitchToBattleState(NodeRemoveEvent e, SingleNode<SelfUserRankSoundEffectInstanceComponent> effect, [JoinAll] SoundListenerNotBattleFinishStateNode listener,
            [JoinAll] SingleNode<MapInstanceComponent> map, [JoinAll] SingleNode<RoundActiveStateComponent> round,
            [JoinAll] ICollection<SingleNode<SelfUserRankSoundEffectInstanceComponent>> effects) {
            if (effects.Count <= 1) {
                ScheduleEvent<SwitchSoundListenerStateEvent<SoundListenerStates.SoundListenerBattleState>>(listener);
            }
        }

        [OnEventFire]
        public void SwitchSoundListenerToBattleState(NodeAddedEvent evt, ContainersScreenNode screen, SingleNode<SoundListenerComponent> listener) {
            ScheduleEvent<SwitchSoundListenerStateEvent<SoundListenerStates.SoundListenerBattleState>>(listener);
        }

        [OnEventFire]
        public void SwitchSoundListenerToLobbyState(NodeRemoveEvent evt, ContainersScreenNode screen, [JoinAll] SingleNode<SoundListenerComponent> listener) {
            ScheduleEvent<SwitchSoundListenerStateEvent<SoundListenerStates.SoundListenerLobbyState>>(listener);
        }

        [OnEventFire]
        public void SwitchSoundListenerToBattleFinishState(DefineMelodyForRoundRestartEvent e, SoundListenerNode listener, [JoinAll] SingleNode<MapInstanceComponent> map) {
            ScheduleEvent<SwitchSoundListenerStateEvent<SoundListenerStates.SoundListenerBattleFinishState>>(listener);
        }

        [OnEventFire]
        public void SwitchSoundListenerToBattleFinishState(StopBattleMelodyWhenRoundBalancedEvent e, SoundListenerNode listener, [JoinAll] SingleNode<MapInstanceComponent> map) {
            ScheduleEvent<SwitchSoundListenerStateEvent<SoundListenerStates.SoundListenerBattleState>>(listener);
        }

        [OnEventFire]
        public void SwitchSoundListenerToLobbyState(LobbyAmbientSoundPlayEvent evt, SoundListenerESMNode soundListener, [JoinAll] Optional<SingleNode<MapInstanceComponent>> map) {
            if (!map.IsPresent()) {
                ScheduleEvent<SwitchSoundListenerStateEvent<SoundListenerStates.SoundListenerLobbyState>>(soundListener);
            }
        }

        [OnEventFire]
        public void SwitchSoundListenerToSpawnState(MapAmbientSoundPlayEvent evt, SoundListenerNotBattleStateESMNode soundListener, [JoinAll] SingleNode<MapInstanceComponent> map) {
            soundListener.soundListenerEsm.Esm.ChangeState<SoundListenerStates.SoundListenerSpawnState>();
            NewEvent<SoundListenerInitBattleStateEvent>().Attach(soundListener).ScheduleDelayed(soundListener.soundListener.DelayForBattleState);
        }

        [OnEventFire]
        public void SwitchSoundListenerToBattleState(SoundListenerInitBattleStateEvent e, SoundListenerESMNode soundListener,
            [JoinSelf] SingleNode<SoundListenerSpawnStateComponent> spawn) {
            ScheduleEvent<SwitchSoundListenerStateEvent<SoundListenerStates.SoundListenerBattleState>>(soundListener);
        }

        [OnEventFire]
        public void SwitchSoundListenerToBattleState(SwitchSoundListenerStateEvent evt, SoundListenerESMNode soundListener) {
            soundListener.soundListenerEsm.Esm.ChangeState(evt.StateType);
        }

        public class ContainersScreenNode : Node {
            public ActiveScreenComponent activeScreen;
            public ContainersScreenComponent containersScreen;
        }

        public class SoundListenerNode : Node {
            public SoundListenerComponent soundListener;
        }

        public class SoundListenerESMNode : SoundListenerNode {
            public SoundListenerESMComponent soundListenerEsm;
        }

        [Not(typeof(SoundListenerBattleStateComponent))]
        public class SoundListenerNotBattleStateESMNode : SoundListenerESMNode { }

        [Not(typeof(SoundListenerBattleFinishStateComponent))]
        public class SoundListenerNotBattleFinishStateNode : Node {
            public SoundListenerBattleStateComponent soundListenerBattleState;
        }
    }
}