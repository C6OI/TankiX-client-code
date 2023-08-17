using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientCore.API {
    public class BattleHUDStates {
        public class ChatState : Node {
            public BattleChatStateComponent battleChatState;
        }

        public class ActionsState : Node {
            public BattleActionsStateComponent battleActionsState;
        }

        public class ShaftAimingState : Node {
            public BattleShaftAimingStateComponent battleShaftAimingState;
        }
    }
}