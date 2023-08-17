using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    [Shared]
    [SerialVersionUID(1438924983080L)]
    public class ExperienceToLevelUpItemComponent : Component {
        public int RemainingExperience { get; set; }

        public int InitLevelExperience { get; set; }

        public int FinalLevelExperience { get; set; }
    }
}