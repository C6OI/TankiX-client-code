using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientUserProfile.Impl {
    public class FractionsCompetitionScoresComponent : Component {
        public Dictionary<long, long> Scores;
        public long TotalCryFund;
    }
}