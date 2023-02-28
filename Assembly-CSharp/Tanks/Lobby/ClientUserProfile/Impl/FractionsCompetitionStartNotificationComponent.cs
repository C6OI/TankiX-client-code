using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientUserProfile.Impl {
    [Shared]
    [SerialVersionUID(1544689451885L)]
    public class FractionsCompetitionStartNotificationComponent : Component {
        public long[] FractionsInCompetition { get; set; }
    }
}