using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.API {
    public class Slots2ModuleBehaviourTypesConfigComponent : Component {
        public Dictionary<Slot, ModuleBehaviourType> Slots { get; set; }
    }
}