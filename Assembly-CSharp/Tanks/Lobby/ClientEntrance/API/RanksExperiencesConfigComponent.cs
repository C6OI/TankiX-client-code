using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientEntrance.API {
    public class RanksExperiencesConfigComponent : Component {
        public int[] RanksExperiences { get; set; }
    }
}