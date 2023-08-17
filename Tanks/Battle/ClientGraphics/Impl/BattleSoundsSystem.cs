using System.Collections.Generic;
using Lobby.ClientSettings.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientGraphics.API;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class BattleSoundsSystem : ECSSystem {
        [OnEventFire]
        public void CleanFirstRoundSpawnWhenExitBattle(NodeRemoveEvent evt, SingleNode<BattleSoundsAssetComponent> mapEffect,
            [JoinAll] SingleNode<RoundFirstSpawnPlayedComponent> listener) =>
            listener.Entity.RemoveComponent<RoundFirstSpawnPlayedComponent>();

        [OnEventFire]
        public void CleanFirstRoundSpawnWhenRoundFinished(NodeRemoveEvent evt, ActiveRoundNode round,
            [JoinByBattle] SelfBattleUserNode battleUser, [JoinAll] SingleNode<RoundFirstSpawnPlayedComponent> listener) =>
            listener.Entity.RemoveComponent<RoundFirstSpawnPlayedComponent>();

        [OnEventFire]
        public void PlaySoundOnFirstSpawn(NodeAddedEvent evt, SpawnSoundsListenerNode listener,
            SingleNode<BattleSoundsAssetComponent> mapEffect, SelfBattleUserNode battleUser,
            [Context] [JoinByBattle] ActiveRoundNode round) => PlaySoundOnFirstSpawn(listener, mapEffect);

        [OnEventFire]
        public void PlaySoundOnFirstSpawn(NodeAddedEvent evt, BattleStateSoundsListenerNode listener,
            SingleNode<BattleSoundsAssetComponent> mapEffect, SelfBattleUserNode battleUser,
            [JoinByBattle] [Context] ActiveRoundNode round) => PlaySoundOnFirstSpawn(listener, mapEffect);

        void PlaySoundOnFirstSpawn(SoundsListenerNode listener, SingleNode<BattleSoundsAssetComponent> mapEffect) {
            if (!listener.Entity.HasComponent<RoundFirstSpawnPlayedComponent>()) {
                listener.Entity.AddComponent<RoundFirstSpawnPlayedComponent>();
                mapEffect.component.BattleSoundsBehaviour.PlayNonTeamSound(listener.soundListener.transform);
            }
        }

        [OnEventFire]
        public void PlaySoundOnRoundFinished(NodeAddedEvent evt, BattleStateSoundsListenerNode listener,
            SingleNode<BattleSoundsAssetComponent> mapEffect, SelfBattleUserNode battleUser,
            [Context] [JoinByBattle] RoundRestartingNode roundRestarting, [JoinByBattle] [Context] BattleNode battle) =>
            NewEvent<DefineRoundRestartSoundEvent>().AttachAll(battleUser, battle, mapEffect, listener).Schedule();

        [OnEventFire]
        public void PlayNonTeamRestartingSoundWhenSpectator(DefineRoundRestartSoundEvent evt, SoundsListenerNode listener,
            SelfSpectatorBattleUser battleUser, SingleNode<BattleSoundsAssetComponent> mapEffect) =>
            mapEffect.component.BattleSoundsBehaviour.PlayNonTeamSound(listener.soundListener.transform);

        [OnEventFire]
        public void PlayNonTeamRestartingSoundWhenTankBattleUserInDM(DefineRoundRestartSoundEvent evt,
            SoundsListenerNode listener, SelfTankBattleUserNode battleUser, DMBattleNode dm,
            SingleNode<BattleSoundsAssetComponent> mapEffect) =>
            mapEffect.component.BattleSoundsBehaviour.PlayNonTeamSound(listener.soundListener.transform);

        [OnEventFire]
        public void PlayNonTeamRestartingSoundWhenTankBattleUserInTeamMode(DefineRoundRestartSoundEvent evt,
            SoundsListenerNode listener, SingleNode<BattleSoundsAssetComponent> mapEffect,
            SelfTankBattleUserInTeamNode battleUser, [JoinByTeam] TeamNode userTeam, TeamBattleNode teamBattle,
            [JoinByBattle] ICollection<TeamNode> teams) {
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
                mapEffect.component.BattleSoundsBehaviour.PlayNonTeamSound(listener.soundListener.transform,
                    listener.soundListenerBattleMixerSnapshotTransition.TransitionTimeToSilentAfterRoundFinish);
            } else {
                mapEffect.component.BattleSoundsBehaviour.PlayTeamSound(listener.soundListener.transform,
                    winSound,
                    listener.soundListenerBattleMixerSnapshotTransition.TransitionTimeToSilentAfterRoundFinish);
            }
        }

        public class RoundNode : Node {
            public BattleGroupComponent battleGroup;
            public RoundComponent round;
        }

        public class ActiveRoundNode : RoundNode {
            public RoundActiveStateComponent roundActiveState;
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

        public class TeamBattleNode : BattleNode {
            public TeamBattleComponent teamBattle;
        }

        public class RoundRestartingNode : RoundNode {
            public RoundRestartingStateComponent roundRestartingState;
        }

        public class TeamNode : Node {
            public BattleGroupComponent battleGroup;

            public TeamColorComponent teamColor;
            public TeamGroupComponent teamGroup;

            public TeamScoreComponent teamScore;
        }

        public class SoundsListenerNode : Node {
            public SoundListenerComponent soundListener;

            public SoundListenerBattleMixerSnapshotTransitionComponent soundListenerBattleMixerSnapshotTransition;
        }

        public class SpawnSoundsListenerNode : SoundsListenerNode {
            public SoundListenerSpawnStateComponent soundListenerSpawnState;
        }

        public class BattleStateSoundsListenerNode : SoundsListenerNode {
            public SoundListenerBattleStateComponent soundListenerBattleState;
        }
    }
}