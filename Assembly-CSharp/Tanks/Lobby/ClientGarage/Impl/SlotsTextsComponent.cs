using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class SlotsTextsComponent : Component {
        public Dictionary<ModuleBehaviourType, string> Slot2modules { get; set; }
    }
}