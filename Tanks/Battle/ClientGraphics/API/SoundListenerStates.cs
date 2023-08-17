using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientGraphics.API {
    public static class SoundListenerStates {
        public class SoundListenerSpawnState : Node {
            public SoundListenerSpawnStateComponent soundListenerSpawnState;
        }

        public class SoundListenerBattleState : Node {
            public SoundListenerBattleStateComponent soundListenerBattleState;
        }

        public class SoundListenerLobbyState : Node {
            public SoundListenerLobbyStateComponent soundListenerLobbyState;
        }
    }
}