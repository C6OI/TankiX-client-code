namespace Tanks.Lobby.ClientUserProfile.API {
    public class WaitingForInviteToLobbyAnswerUIComponent : WaitingAnswerUIComponent {
        public bool AlreadyInLobby {
            set {
                Waiting = false;

                if (value) {
                    inviteButton.SetActive(false);
                }

                alreadyInLabel.SetActive(value);
            }
        }
    }
}