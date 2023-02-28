using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.API {
    public class TutorialRequiredCompletedTutorialsComponent : Component {
        public List<long> TutorialsIds { get; set; }
    }
}