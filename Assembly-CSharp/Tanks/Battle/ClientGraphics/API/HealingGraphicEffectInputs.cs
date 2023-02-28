using Platform.Kernel.ECS.ClientEntitySystem.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.API {
    public class HealingGraphicEffectInputs {
        public HealingGraphicEffectInputs(Entity entity, SkinnedMeshRenderer renderer) {
            Entity = entity;
            Renderer = renderer;
        }

        public SkinnedMeshRenderer Renderer { get; }

        public virtual float TilingX => 4f;

        public Entity Entity { get; }
    }
}