using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientMatchMaking.Impl {
    public class PlayButtonStates {
        public const string NORMAL_ANIMATOR_STATE_TRIGGER = "Normal";

        public const string SEARCHING_ANIMATOR_STATE_TRIGGER = "Searching";

        public const string ENTERING_LOBBY_ANIMATOR_STATE_TRIGGER = "EnteringLobby";

        public const string MATCH_BEGIN_TIMER_ANIMATOR_STATE_TRIGGER = "MatchBeginTimer";

        public const string NOT_ENOUGHT_PLAYERS_TIMER_ANIMATOR_STATE_TRIGGER = "NotEnoughtPlayersTimer";

        public const string MATCH_BEGINNING_ANIMATOR_STATE_TRIGGER = "MatchBegining";

        public const string CUSTOM_BATTLE_ANIMATOR_STATE_TRIGGER = "CustomBattle";

        public const string START_CUSTOM_BATTLE_ANIMATOR_STATE_TRIGGER = "StartCustomBattle";

        public const string RETURN_TO_BATTLE_ANIMATOR_STATE_TRIGGER = "ReturnToBattle";

        public const string ENERGY_SHARE_STATE_ANIMATOR_STATE_TRIGGER = "EnergyShareState";

        public class NormalState : Node {
            public PlayButtonNormalStateComponent playButtonNormalState;
        }

        public class SearchingState : Node {
            public PlayerButtonSearchingStateComponent playerButtonSearchingState;
        }

        public class EnteringLobbyState : Node {
            public PlayerButtonEnteringLobbyStateComponent playerButtonEnteringLobbyState;
        }

        public class MatchBeginTimerState : Node {
            public PlayerButtonMatchBeginTimerStateComponent playerButtonMatchBeginTimerState;
        }

        public class NotEnoughtPlayersState : Node {
            public PlayButtonNotEnoughtPlayersTimerStateComponent playButtonNotEnoughtPlayersTimerState;
        }

        public class MatchBeginningState : Node {
            public PlayButtonMatchBeginningStateComponent playButtonMatchBeginningState;
        }

        public class CustomBattleState : Node {
            public PlayButtonCustomBattleStateComponent playButtonCustomBattleState;
        }

        public class StartCustomBattleState : Node {
            public PlayButtonStartCustomBattleStateComponent playButtonStartCustomBattleState;
        }

        public class ReturnToBattleState : Node {
            public PlayButtonReturnToBattleStateComponent playButtonReturnToBattleState;
        }

        public class EnergyShareScreenState : Node {
            public PlayButtonEnergyShareScreenStateComponent playButtonEnergyShareScreenState;
        }
    }
}