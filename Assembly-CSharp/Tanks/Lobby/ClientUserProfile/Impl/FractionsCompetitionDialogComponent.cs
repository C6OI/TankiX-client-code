using Tanks.Lobby.ClientGarage.Impl;

namespace Tanks.Lobby.ClientUserProfile.Impl {
    public class FractionsCompetitionDialogComponent : ConfirmDialogComponent {
        public FractionSelectWindowComponent FractionSelectWindow;

        public CurrentCompetitionWindowComponent CurrentCompetitionWindow;

        public CompetitionAwardWindowComponent CompetitionAwardWindow;

        public FractionLearnMoreWindowComponent LearnMoreWindow;

        public override void Hide() {
            FractionSelectWindow.gameObject.SetActive(false);
            CurrentCompetitionWindow.gameObject.SetActive(false);
            CompetitionAwardWindow.gameObject.SetActive(false);
            LearnMoreWindow.gameObject.SetActive(false);
            base.Hide();
        }
    }
}