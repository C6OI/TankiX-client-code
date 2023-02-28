using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientMatchMaking.Impl {
    public class PlayButtonViewSystem : ECSSystem {
        [OnEventFire]
        public void ToNormalState(NodeAddedEvent e, NormalStateNode node) {
            node.playButton.StopTheStopwatch();
            node.playButton.SetAnimatorTrigger("Normal");
        }

        [OnEventFire]
        public void ToSearchingState(NodeAddedEvent e, SearchingStateNode node) {
            node.playButton.RunTheStopwatch();
            node.playButton.SetAnimatorTrigger("Searching");
        }

        [OnEventFire]
        public void ToEnteringLobbyState(NodeAddedEvent e, EnteringLobbyStateNode node) {
            node.playButton.StopTheStopwatch();
            node.playButton.SetAnimatorTrigger("EnteringLobby");
        }

        [OnEventFire]
        public void ToMatchBeginTimerState(NodeAddedEvent e, MatchBeginTimerStateNode node) {
            node.playButton.StopTheStopwatch();
            node.playButton.SetAnimatorTrigger("MatchBeginTimer");
        }

        [OnEventFire]
        public void ToNotEnoughPlayersTimerState(NodeAddedEvent e, NotEnoughtPlayersStateNode node) {
            node.playButton.StopTheStopwatch();
            node.playButton.SetAnimatorTrigger("NotEnoughtPlayersTimer");
        }

        [OnEventFire]
        public void PlayButtonTimerExpired(PlayButtonTimerExpiredEvent e, MatchBeginTimerStateNode node) {
            node.Entity.GetComponent<ESMComponent>().Esm.ChangeState<PlayButtonStates.MatchBeginningState>();
        }

        [OnEventFire]
        public void ToMatchBeginnigState(NodeAddedEvent e, MatchBeginningStateNode button) {
            button.playButton.StopTheTimer();
            button.playButton.SetAnimatorTrigger("MatchBegining");
        }

        [OnEventFire]
        public void ToCustomBattleState(NodeAddedEvent e, CustomBattleStateNode button) {
            button.playButton.SetAnimatorTrigger("CustomBattle");
        }

        [OnEventFire]
        public void ToStartCustomBattleState(NodeAddedEvent e, StartCustomBattleStateNode button) {
            button.playButton.SetAnimatorTrigger("StartCustomBattle");
        }

        [OnEventFire]
        public void ToReturnToBattleState(NodeAddedEvent e, ReturnToBattleStateNode button) {
            button.playButton.SetAnimatorTrigger("ReturnToBattle");
        }

        [OnEventFire]
        public void ToReturnToBattleState(NodeAddedEvent e, ShareEnergyStateNode button) {
            button.playButton.SetAnimatorTrigger("EnergyShareState");
        }

        public class NormalStateNode : Node {
            public PlayButtonComponent playButton;
            public PlayButtonNormalStateComponent playButtonNormalState;
        }

        public class SearchingStateNode : Node {
            public PlayButtonComponent playButton;
            public PlayerButtonSearchingStateComponent playerButtonSearchingState;
        }

        public class EnteringLobbyStateNode : Node {
            public PlayButtonComponent playButton;
            public PlayerButtonEnteringLobbyStateComponent playerButtonEnteringLobbyState;
        }

        public class MatchBeginTimerStateNode : Node {
            public ESMComponent esm;

            public PlayButtonComponent playButton;
            public PlayerButtonMatchBeginTimerStateComponent playerButtonMatchBeginTimerState;
        }

        public class NotEnoughtPlayersStateNode : Node {
            public PlayButtonComponent playButton;
            public PlayButtonNotEnoughtPlayersTimerStateComponent playButtonNotEnoughtPlayersTimerState;
        }

        public class MatchBeginningStateNode : Node {
            public PlayButtonComponent playButton;
            public PlayButtonMatchBeginningStateComponent playButtonMatchBeginningState;
        }

        public class CustomBattleStateNode : Node {
            public PlayButtonComponent playButton;
            public PlayButtonCustomBattleStateComponent playButtonCustomBattleState;
        }

        public class StartCustomBattleStateNode : Node {
            public PlayButtonComponent playButton;
            public PlayButtonStartCustomBattleStateComponent playButtonStartCustomBattleState;
        }

        public class ReturnToBattleStateNode : Node {
            public PlayButtonComponent playButton;
            public PlayButtonReturnToBattleStateComponent playButtonReturnToBattleState;
        }

        public class ShareEnergyStateNode : Node {
            public PlayButtonComponent playButton;
            public PlayButtonEnergyShareScreenStateComponent playButtonEnergyShareScreenState;
        }
    }
}