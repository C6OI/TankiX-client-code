using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientGraphics.API {
    public static class SoundListenerStates {
        public class SoundListenerSpawnState : Node {
            public SoundListenerSpawnStateComponent soundListenerSpawnState;
        }

        public class SoundListenerBattleState : Node {
            public SoundListenerBattleStateComponent soundListenerBattleState;
            public SoundListenerReadyForHitFeedbackComponent soundListenerReadyForHitFeedback;
        }

        public class SoundListenerBattleFinishState : Node {
            public SoundListenerBattleFinishStateComponent soundListenerBattleFinishState;
            public SoundListenerBattleStateComponent soundListenerBattleState;
        }

        public class SoundListenerSelfRankRewardState : Node {
            public SoundListenerBattleStateComponent soundListenerBattleState;

            public SoundListenerSelfRankRewardStateComponent soundListenerSelfRankRewardState;
        }

        public class SoundListenerLobbyState : Node {
            public SoundListenerLobbyStateComponent soundListenerLobbyState;
        }
    }
}