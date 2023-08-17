using System.Collections.Generic;
using Lobby.ClientSettings.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine.Audio;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class SoundListenerBattleSnapshotsSystem : ECSSystem {
        [OnEventFire]
        public void SwitchToSilentWhenSpawnState(NodeAddedEvent e, SoundListenerSpawnStateNode listener) =>
            SwitchToSilent(listener, 0f);

        [OnEventFire]
        public void SwitchToSilentWhenRoundFinish(NodeRemoveEvent e, SingleNode<RoundActiveStateComponent> roundActive,
            [JoinSelf] RoundNode round, [JoinByBattle] SelfBattleUserNode battleUser,
            [JoinAll] SoundListenerNode listener) => SwitchToSilent(listener,
            listener.soundListenerBattleMixerSnapshotTransition.TransitionTimeToSilentAfterRoundFinish);

        [OnEventFire]
        public void SwitchToSilentWhenExitBattle(ExitBattleEvent e, Node node, [JoinAll] SoundListenerNode listener) =>
            SwitchToSilent(listener,
                listener.soundListenerBattleMixerSnapshotTransition.TransitionTimeToSilentAfterExitBattle);

        [OnEventFire]
        public void SwitchToLoudWhenBattleState(NodeAddedEvent e, SoundListenerBattleStateNode listener) =>
            SwitchToLoud(listener, listener.soundListenerBattleMixerSnapshotTransition.TransitionToLoudTimeInBattle);

        [OnEventFire]
        public void SwitchToLoudWhenNewRoundInBattle(NodeAddedEvent e, ActiveRoundNode round,
            [JoinByBattle] [Context] SelfBattleUserNode battleUser, [JoinAll] SoundListenerBattleStateNode listener) =>
            SwitchToLoud(listener, listener.soundListenerBattleMixerSnapshotTransition.TransitionToLoudTimeInBattle);

        [OnEventFire]
        public void SwitchToSelfUserSnapshot(NodeAddedEvent e, SingleNode<SelfUserRankSoundEffectInstanceComponent> effect,
            [JoinAll] SoundListenerBattleStateNode listener, [JoinAll] SingleNode<MapInstanceComponent> map,
            [JoinAll] SingleNode<RoundActiveStateComponent> round) => Switch(listener,
            listener.soundListenerResources.Resources.SfxMixerSnapshots[listener.soundListenerBattleMixerSnapshots
                .SelfUserSnapshotIndex],
            0f);

        [OnEventFire]
        public void SwitchToLoudFromUser(NodeRemoveEvent e, SingleNode<SelfUserRankSoundEffectInstanceComponent> effect,
            [JoinAll] SoundListenerBattleStateNode listener, [JoinAll] SingleNode<MapInstanceComponent> map,
            [JoinAll] SingleNode<RoundActiveStateComponent> round,
            [JoinAll] ICollection<SingleNode<SelfUserRankSoundEffectInstanceComponent>> effects) {
            if (effects.Count <= 1) {
                Switch(listener,
                    listener.soundListenerResources.Resources.SfxMixerSnapshots[listener.soundListenerBattleMixerSnapshots
                        .LoudSnapshotIndex],
                    listener.soundListenerBattleMixerSnapshotTransition.TransitionToLoudTimeInSelfUserMode);
            }
        }

        void SwitchToLoud(SoundListenerNode listener, float transitionTime) => Switch(listener,
            listener.soundListenerResources.Resources.SfxMixerSnapshots[listener.soundListenerBattleMixerSnapshots
                .LoudSnapshotIndex],
            transitionTime);

        void SwitchToSilent(SoundListenerNode listener, float transitionTime) => Switch(listener,
            listener.soundListenerResources.Resources.SfxMixerSnapshots[listener.soundListenerBattleMixerSnapshots
                .SilentSnapshotIndex],
            transitionTime);

        void Switch(SoundListenerNode listener, AudioMixerSnapshot snapshot, float transition) {
            SoundListenerResourcesBehaviour resources = listener.soundListenerResources.Resources;

            resources.SfxMixer.TransitionToSnapshots(new AudioMixerSnapshot[1] { snapshot },
                new float[1] { 1f },
                transition);
        }

        public class SoundListenerNode : Node {
            public SoundListenerComponent soundListener;

            public SoundListenerBattleMixerSnapshotsComponent soundListenerBattleMixerSnapshots;

            public SoundListenerBattleMixerSnapshotTransitionComponent soundListenerBattleMixerSnapshotTransition;

            public SoundListenerResourcesComponent soundListenerResources;
        }

        public class SoundListenerBattleStateNode : SoundListenerNode {
            public SoundListenerBattleStateComponent soundListenerBattleState;
        }

        public class SoundListenerSpawnStateNode : SoundListenerNode {
            public SoundListenerSpawnStateComponent soundListenerSpawnState;
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

            public BattleUserComponent battleUser;
            public SelfBattleUserComponent selfBattleUser;
        }
    }
}