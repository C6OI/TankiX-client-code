using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.API {
    public class ModuleEffectsComponent : Component {
        public List<EffectConfig> Effects { get; set; }
    }
}