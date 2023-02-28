using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1476865927439L)]
    public class ItemUpgradeExperiencesConfigComponent : Component {
        public int[] LevelsExperiences { get; set; }
    }
}