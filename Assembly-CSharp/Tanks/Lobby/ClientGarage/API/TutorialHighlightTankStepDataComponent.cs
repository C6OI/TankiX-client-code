using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.API {
    public class TutorialHighlightTankStepDataComponent : Component {
        public bool HighlightHull { get; set; }

        public bool HighlightWeapon { get; set; }
    }
}