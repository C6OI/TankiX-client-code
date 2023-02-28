using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientUserProfile.Impl {
    [SerialVersionUID(1534913762047L)]
    public interface FractionsCompetitionTemplate : Template {
        [AutoAdded]
        [PersistentConfig]
        FractionsCompetitionInfoComponent fractionCompetitionInfo();

        [AutoAdded]
        FractionsCompetitionScoresComponent fractionCompetitionScores();
    }
}