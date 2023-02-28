using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class ModuleTypesImagesComponent : Component {
        public Dictionary<ModuleBehaviourType, string> moduleType2image { get; set; }

        public Dictionary<ModuleBehaviourType, string> moduleType2color { get; set; }
    }
}