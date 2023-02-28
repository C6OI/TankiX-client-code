using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class CharacterShadowInternalComponent : Component {
        public Projector Projector { get; set; }

        public Material CasterMaterial { get; set; }

        public float BaseAlpha { get; set; }

        public Bounds ProjectionBoundInLightSpace { get; set; }
    }
}