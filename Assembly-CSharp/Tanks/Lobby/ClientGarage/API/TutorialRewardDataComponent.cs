using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.API {
    public class TutorialRewardDataComponent : Component {
        public long CrysCount { get; set; }

        public List<Reward> Rewards { get; set; }
    }
}