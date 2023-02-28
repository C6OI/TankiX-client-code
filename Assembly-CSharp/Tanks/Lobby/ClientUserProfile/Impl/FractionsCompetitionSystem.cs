using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientUserProfile.Impl {
    public class FractionsCompetitionSystem : ECSSystem {
        [OnEventFire]
        public void UpdateScores(UpdateClientFractionScoresEvent e, FractionCompetitionNode competition) {
            competition.fractionsCompetitionScores.TotalCryFund = e.TotalCryFund;
            competition.fractionsCompetitionScores.Scores = e.Scores;
        }

        public class FractionCompetitionNode : Node {
            public FractionsCompetitionInfoComponent fractionsCompetitionInfo;

            public FractionsCompetitionScoresComponent fractionsCompetitionScores;
        }
    }
}