using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientUserProfile.Impl {
    public class FractionsCompetitionInfoComponent : Component {
        public string CompetitionTitle { get; set; }

        public string CompetitionStartMessage { get; set; }

        public string CompetitionDescription { get; set; }

        public string CompetitionLogoUid { get; set; }

        public string MainQuestionMessage { get; set; }

        public string TakePartButtonText { get; set; }

        public string WinnerFinishMessage { get; set; }

        public string LoserFinishMessage { get; set; }

        public string HereYourRewardMessage { get; set; }

        public string RewardsButtonText { get; set; }
    }
}