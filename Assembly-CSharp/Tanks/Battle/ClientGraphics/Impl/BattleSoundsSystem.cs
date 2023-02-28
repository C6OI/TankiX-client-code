using System.Collections.Generic;
using System.Linq;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientGraphics.API;
using Tanks.Lobby.ClientSettings.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class BattleSoundsSystem : ECSSystem {
        [OnEventFire]
        public void CleanFirstRoundSpawnWhenExitBattle(NodeRemoveEvent evt, SingleNode<BattleSoundsAssetComponent> mapEffect,
            [JoinAll] SingleNode<RoundFirstSpawnPlayedComponent> listener) {
            listener.Entity.RemoveComponent<RoundFirstSpawnPlayedComponent>();
        }

        [OnEventFire]
        public void CleanFirstRoundSpawnWhenRoundFinished(NodeRemoveEvent evt, ActiveRoundNode round, [JoinByBattle] SelfBattleUserNode battleUser,
            [JoinAll] SingleNode<RoundFirstSpawnPlayedComponent> listener) {
            listener.Entity.RemoveComponent<RoundFirstSpawnPlayedComponent>();
        }

        [OnEventFire]
        public void PlaySoundOnFirstSpawn(NodeAddedEvent evt, SpawnSoundsListenerNode listener, SingleNode<BattleSoundsAssetComponent> mapEffect, SelfBattleUserNode battleUser,
            [JoinByBattle] [Context] ActiveRoundNode round) {
            PlaySoundOnFirstSpawn(listener, mapEffect);
        }

        [OnEventFire]
        public void PlaySoundOnFirstSpawn(NodeAddedEvent evt, BattleStateSoundsListenerNode listener, SingleNode<BattleSoundsAssetComponent> mapEffect, SelfBattleUserNode battleUser,
            [JoinByBattle] [Context] ActiveRoundNode round) {
            PlaySoundOnFirstSpawn(listener, mapEffect);
        }

        void PlaySoundOnFirstSpawn(SoundsListenerNode listener, SingleNode<BattleSoundsAssetComponent> mapEffect) {
            if (!listener.Entity.HasComponent<RoundFirstSpawnPlayedComponent>()) {
                listener.Entity.AddComponent<RoundFirstSpawnPlayedComponent>();
                mapEffect.component.BattleSoundsBehaviour.PlayStartSound(listener.soundListener.transform);
            }
        }

        [OnEventFire]
        public void SetOldSoundsForListenerWhenEnterBattle(NodeAddedEvent evt, SoundsListenerNode listener, SelfBattleUserNode battleUser, [JoinByBattle] [Context] ActiveRoundNode round,
            SingleNode<BattleSoundsAssetComponent> mapEffect) {
            listener.Entity.AddComponent<OldRoundRestartSoundsListenerComponent>();
        }

        [OnEventFire]
        public void CheckRemainingTimeInRound(NodeAddedEvent evt, SoundsListenerWithOldRoundRestartSoundsNode listener, SelfBattleUserNode battleUser,
            [JoinByBattle] [Context] ActiveRoundStopTimeNode round, SingleNode<BattleSoundsAssetComponent> mapEffect) {
            float num = round.roundStopTime.StopTime.UnityTime - Date.Now.UnityTime;

            if (!(num < mapEffect.component.BattleSoundsBehaviour.MinRemainigRoundTimeSec)) {
                listener.Entity.RemoveComponent<OldRoundRestartSoundsListenerComponent>();
                listener.Entity.AddComponent<MelodiesRoundRestartListenerComponent>();
            }
        }

        [OnEventFire]
        public void CleanOldSoundsMarkerWhenExitBattle(NodeRemoveEvent evt, SingleNode<BattleSoundsAssetComponent> mapEffect,
            [JoinAll] SingleNode<OldRoundRestartSoundsListenerComponent> listener) {
            listener.Entity.RemoveComponent<OldRoundRestartSoundsListenerComponent>();
        }

        [OnEventFire]
        public void CleanMelodiesMarkerWhenExitBattle(NodeRemoveEvent evt, SingleNode<BattleSoundsAssetComponent> mapEffect,
            [JoinAll] SingleNode<MelodiesRoundRestartListenerComponent> listener) {
            listener.Entity.RemoveComponent<MelodiesRoundRestartListenerComponent>();
        }

        [OnEventFire]
        public void CleanOldSoundsMarkerWhenRoundRestart(NodeRemoveEvent evt, ActiveRoundNode activeRound, [JoinByBattle] SelfBattleUserNode battleUser,
            [JoinAll] SingleNode<OldRoundRestartSoundsListenerComponent> listener) {
            listener.Entity.RemoveComponent<OldRoundRestartSoundsListenerComponent>();
        }

        [OnEventFire]
        public void CleanMelodiesMarkerWhenRoundRestart(NodeRemoveEvent evt, ActiveRoundNode activeRound, [JoinByBattle] SelfBattleUserNode battleUser,
            [JoinAll] SingleNode<MelodiesRoundRestartListenerComponent> listener) {
            listener.Entity.RemoveComponent<MelodiesRoundRestartListenerComponent>();
        }

        [OnEventFire]
        public void CheckRoundTimer(UpdateEvent e, SoundsListenerWithRoundRestartMelodiesNode listener, [JoinAll] SelfBattleUserNode battleUser,
            [JoinByBattle] ActiveRoundStopTimeNode round, [JoinAll] SingleNode<BattleSoundsAssetComponent> mapEffect) {
            float num = round.roundStopTime.StopTime.UnityTime - Date.Now.UnityTime;

            if (!(num > mapEffect.component.BattleSoundsBehaviour.MinRemainigRoundTimeSec)) {
                NewEvent<DefineMelodyForRoundRestartEvent>().AttachAll(battleUser, mapEffect, listener, round).Schedule();
                listener.Entity.RemoveComponent<MelodiesRoundRestartListenerComponent>();
            }
        }

        [OnEventFire]
        public void RoundDisbalanced(UpdateEvent e, SingleNode<RoundDisbalancedComponent> round, [JoinAll] SoundsListenerWithRoundRestartMelodiesNode listener,
            [JoinAll] SelfBattleUserNode battleUser, [JoinAll] SingleNode<BattleSoundsAssetComponent> mapEffect) {
            float num = round.component.FinishTime.UnityTime - Date.Now.UnityTime;

            if (!(num > mapEffect.component.BattleSoundsBehaviour.MinRemainigRoundTimeSec)) {
                NewEvent<DefineMelodyForRoundRestartEvent>().AttachAll(battleUser, mapEffect, listener, round).Schedule();
                listener.Entity.RemoveComponent<MelodiesRoundRestartListenerComponent>();
            }
        }

        [OnEventFire]
        public void RoundBalanced(NodeRemoveEvent e, SingleNode<RoundDisbalancedComponent> round, [JoinAll] SoundsListenerNode listener, [JoinAll] SelfBattleUserNode battleUser,
            [JoinAll] SingleNode<BattleSoundsAssetComponent> mapEffect) {
            if (!listener.Entity.HasComponent<MelodiesRoundRestartListenerComponent>()) {
                listener.Entity.AddComponent<MelodiesRoundRestartListenerComponent>();
            }

            ScheduleEvent<StopBattleMelodyWhenRoundBalancedEvent>(listener);
        }

        [OnEventFire]
        public void CleanPlayingMelody(DefineMelodyForRoundRestartEvent evt, SingleNode<PlayingMelodyRoundRestartListenerComponent> listener) {
            listener.Entity.RemoveComponent<PlayingMelodyRoundRestartListenerComponent>();
        }

        [OnEventComplete]
        public void PlayNeutralMelodyWhenSpectator(DefineMelodyForRoundRestartEvent evt, SoundsListenerNode listener, SelfSpectatorBattleUser battleUser,
            SingleNode<BattleSoundsAssetComponent> mapEffect) {
            listener.Entity.AddComponent(new PlayingMelodyRoundRestartListenerComponent(mapEffect.component.BattleSoundsBehaviour.PlayNeutralMelody(listener.soundListener.transform)));
        }

        [OnEventComplete]
        public void PlayMelodyWhenSelfBattleUserInDM(DefineMelodyForRoundRestartEvent evt, SoundsListenerNode listener, SelfTankBattleUserNode battleUser,
            [JoinByUser] RoundUserNode selfRoundUser, [JoinByBattle] DMBattleNode dm, [JoinByBattle] ICollection<RoundUserNode> players,
            SingleNode<BattleSoundsAssetComponent> mapEffect) {
            List<RoundUserNode> list = players.ToList();

            if (list.Count == 1) {
                listener.Entity.AddComponent(
                    new PlayingMelodyRoundRestartListenerComponent(mapEffect.component.BattleSoundsBehaviour.PlayNeutralMelody(listener.soundListener.transform)));

                return;
            }

            list.Sort((RoundUserNode r1, RoundUserNode r2) => r1.roundUserStatistics.CompareTo(r2.roundUserStatistics));
            RoundUserNode roundUserNode = list[0];

            if (!roundUserNode.Entity.Equals(selfRoundUser.Entity)) {
                listener.Entity.AddComponent(
                    new PlayingMelodyRoundRestartListenerComponent(mapEffect.component.BattleSoundsBehaviour.PlayNeutralMelody(listener.soundListener.transform)));
            } else if (Mathf.Abs(roundUserNode.roundUserStatistics.ScoreWithoutBonuses - list[1].roundUserStatistics.ScoreWithoutBonuses) <
                       mapEffect.component.BattleSoundsBehaviour.MinDmScoreDiff) {
                listener.Entity.AddComponent(
                    new PlayingMelodyRoundRestartListenerComponent(mapEffect.component.BattleSoundsBehaviour.PlayNeutralMelody(listener.soundListener.transform)));
            } else {
                listener.Entity.AddComponent(
                    new PlayingMelodyRoundRestartListenerComponent(mapEffect.component.BattleSoundsBehaviour.PlayNonNeutralMelody(listener.soundListener.transform, true)));
            }
        }

        [OnEventComplete]
        public void PlayMelodyWhenSelfBattleUserInTDM(DefineMelodyForRoundRestartEvent evt, SoundsListenerNode listener, SelfTankBattleUserInTeamNode battleUser,
            [JoinByTeam] TeamNode userTeam, [JoinByBattle] TDMBattleNode dm, [JoinByBattle] ICollection<TeamNode> teams, SingleNode<BattleSoundsAssetComponent> mapEffect) {
            PlayNonTeamMelodyInTeamMode(userTeam, teams, listener, mapEffect, mapEffect.component.BattleSoundsBehaviour.MinTdmScoreDiff);
        }

        void PlayNonTeamMelodyInTeamMode(TeamNode userTeam, ICollection<TeamNode> teams, SoundsListenerNode listener, SingleNode<BattleSoundsAssetComponent> mapEffect,
            int minScoreDiff) {
            Entity userTeamEntity = userTeam.Entity;
            int userTeamScore = userTeam.teamScore.Score;
            bool isNearScores = false;
            bool winSound = true;

            teams.ForEach(delegate(TeamNode t) {
                if (!ReferenceEquals(t.Entity, userTeamEntity)) {
                    int score = t.teamScore.Score;

                    if (Mathf.Abs(score - userTeamScore) < minScoreDiff) {
                        isNearScores = true;
                    }

                    if (score > userTeamScore) {
                        winSound = false;
                    }
                }
            });

            if (isNearScores) {
                listener.Entity.AddComponent(
                    new PlayingMelodyRoundRestartListenerComponent(mapEffect.component.BattleSoundsBehaviour.PlayNeutralMelody(listener.soundListener.transform)));
            } else {
                listener.Entity.AddComponent(
                    new PlayingMelodyRoundRestartListenerComponent(mapEffect.component.BattleSoundsBehaviour.PlayNonNeutralMelody(listener.soundListener.transform, winSound)));
            }
        }

        [OnEventComplete]
        public void PlayMelodyWhenSelfBattleUserInCTF(DefineMelodyForRoundRestartEvent evt, SoundsListenerNode listener, SelfTankBattleUserInTeamNode battleUser,
            [JoinByTeam] TeamNode userTeam, [JoinByBattle] CTFBattleNode dm, [JoinByBattle] ICollection<TeamNode> teams, SingleNode<BattleSoundsAssetComponent> mapEffect) {
            PlayNonTeamMelodyInTeamMode(userTeam, teams, listener, mapEffect, mapEffect.component.BattleSoundsBehaviour.MinCtfScoreDiff);
        }

        [OnEventFire]
        public void FinalizeAmbientMapSoundEffect(LobbyAmbientSoundPlayEvent evt, SingleNode<PlayingMelodyRoundRestartListenerComponent> listener) {
            ScheduleEvent<StopBattleMelodyEvent>(listener);
        }

        [OnEventFire]
        public void StopMelody(StopBattleMelodyEvent evt, SingleNode<PlayingMelodyRoundRestartListenerComponent> listener) {
            listener.Entity.RemoveComponent<PlayingMelodyRoundRestartListenerComponent>();
        }

        [OnEventFire]
        public void StopMelody(NodeRemoveEvent evt, SingleNode<PlayingMelodyRoundRestartListenerComponent> listener) {
            listener.component.Melody.Stop();
        }

        [OnEventFire]
        public void PlaySoundOnRoundFinished(NodeAddedEvent evt, BattleStateSoundsListenerNode listener, SoundsListenerWithOldRoundRestartSoundsNode listenerWithOldRoundRestartSounds,
            SingleNode<BattleSoundsAssetComponent> mapEffect, SelfBattleUserNode battleUser, [Context] [JoinByBattle] RoundRestartingNode roundRestarting,
            [Context] [JoinByBattle] BattleNode battle) {
            NewEvent<DefineRoundRestartSoundEvent>().AttachAll(battleUser, battle, mapEffect, listener).Schedule();
        }

        [OnEventFire]
        public void PlayNonTeamRestartingSoundWhenSpectator(DefineRoundRestartSoundEvent evt, SoundsListenerNode listener, SelfSpectatorBattleUser battleUser,
            SingleNode<BattleSoundsAssetComponent> mapEffect) {
            mapEffect.component.BattleSoundsBehaviour.PlayShortNeutralSound(listener.soundListener.transform);
        }

        [OnEventFire]
        public void PlayNonTeamRestartingSoundWhenTankBattleUserInDM(DefineRoundRestartSoundEvent evt, SoundsListenerNode listener, SelfTankBattleUserNode battleUser, DMBattleNode dm,
            SingleNode<BattleSoundsAssetComponent> mapEffect) {
            mapEffect.component.BattleSoundsBehaviour.PlayShortNeutralSound(listener.soundListener.transform);
        }

        [OnEventFire]
        public void PlayNonTeamRestartingSoundWhenTankBattleUserInTeamMode(DefineRoundRestartSoundEvent evt, SoundsListenerNode listener, SingleNode<BattleSoundsAssetComponent> mapEffect,
            SelfTankBattleUserInTeamNode battleUser, [JoinByTeam] TeamNode userTeam, TeamBattleNode teamBattle, [JoinByBattle] ICollection<TeamNode> teams) {
            Entity userTeamEntity = userTeam.Entity;
            int userTeamScore = userTeam.teamScore.Score;
            bool isEqualScore = true;
            bool winSound = true;

            teams.ForEach(delegate(TeamNode t) {
                if (!ReferenceEquals(t.Entity, userTeamEntity)) {
                    int score = t.teamScore.Score;

                    if (score != userTeamScore) {
                        isEqualScore = false;
                    }

                    if (score > userTeamScore) {
                        winSound = false;
                    }
                }
            });

            if (isEqualScore) {
                mapEffect.component.BattleSoundsBehaviour.PlayShortNeutralSound(listener.soundListener.transform,
                    listener.soundListenerBattleMixerSnapshotTransition.TransitionTimeToSilentAfterRoundFinish);
            } else {
                mapEffect.component.BattleSoundsBehaviour.PlayShortNonNeutralSound(listener.soundListener.transform, winSound,
                    listener.soundListenerBattleMixerSnapshotTransition.TransitionTimeToSilentAfterRoundFinish);
            }
        }

        public class RoundNode : Node {
            public BattleGroupComponent battleGroup;
            public RoundComponent round;
        }

        public class RoundUserNode : Node {
            public RoundUserStatisticsComponent roundUserStatistics;
        }

        public class ActiveRoundNode : RoundNode {
            public RoundActiveStateComponent roundActiveState;
        }

        public class ActiveRoundStopTimeNode : RoundNode {
            public RoundActiveStateComponent roundActiveState;

            public RoundStopTimeComponent roundStopTime;
        }

        public class SelfBattleUserNode : Node {
            public BattleGroupComponent battleGroup;
            public SelfBattleUserComponent selfBattleUser;
        }

        public class SelfTankBattleUserNode : SelfBattleUserNode {
            public UserInBattleAsTankComponent userInBattleAsTank;
        }

        public class SelfTankBattleUserInTeamNode : SelfTankBattleUserNode {
            public TeamGroupComponent teamGroup;
        }

        public class SelfSpectatorBattleUser : SelfBattleUserNode {
            public UserInBattleAsSpectatorComponent userInBattleAsSpectator;
        }

        public class BattleNode : Node {
            public BattleComponent battle;

            public BattleGroupComponent battleGroup;

            public MapGroupComponent mapGroup;
        }

        public class DMBattleNode : BattleNode {
            public DMComponent dm;
        }

        public class TDMBattleNode : BattleNode {
            public TDMComponent tdm;
        }

        public class CTFBattleNode : BattleNode {
            public CTFComponent ctf;
        }

        public class TeamBattleNode : BattleNode {
            public TeamBattleComponent teamBattle;
        }

        public class RoundRestartingNode : RoundNode {
            public RoundRestartingStateComponent roundRestartingState;
        }

        public class TeamNode : Node {
            public BattleGroupComponent battleGroup;
            public TeamGroupComponent teamGroup;

            public TeamScoreComponent teamScore;
        }

        public class SoundsListenerNode : Node {
            public SoundListenerComponent soundListener;

            public SoundListenerBattleMixerSnapshotTransitionComponent soundListenerBattleMixerSnapshotTransition;
        }

        public class SoundsListenerWithOldRoundRestartSoundsNode : SoundsListenerNode {
            public OldRoundRestartSoundsListenerComponent oldRoundRestartSoundsListener;
        }

        public class SoundsListenerWithRoundRestartMelodiesNode : SoundsListenerNode {
            public MelodiesRoundRestartListenerComponent melodiesRoundRestartListener;
        }

        public class SpawnSoundsListenerNode : SoundsListenerNode {
            public SoundListenerSpawnStateComponent soundListenerSpawnState;
        }

        public class BattleStateSoundsListenerNode : SoundsListenerNode {
            public SoundListenerBattleStateComponent soundListenerBattleState;
        }
    }
}