using System.Collections.Generic;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class PreloadedModuleEffectsComponent : Component {
        public PreloadedModuleEffectsComponent(Dictionary<string, GameObject> preloadedEffects) => PreloadedEffects = preloadedEffects;

        public Dictionary<string, GameObject> PreloadedEffects { get; set; }
    }
}