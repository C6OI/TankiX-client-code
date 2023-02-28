using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class PreloadingModuleEffectsComponent : BehaviourComponent {
        [SerializeField] Transform preloadedModuleEffectsRoot;

        [SerializeField] PreloadingModuleEffectData[] preloadingModuleEffects;

        public Transform PreloadedModuleEffectsRoot => preloadedModuleEffectsRoot;

        public PreloadingModuleEffectData[] PreloadingModuleEffects => preloadingModuleEffects;

        public Dictionary<string, GameObject> PreloadingBuffer { get; set; }

        public List<Entity> EntitiesForEffectsLoading { get; set; }
    }
}