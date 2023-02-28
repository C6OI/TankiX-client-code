using Platform.Kernel.ECS.ClientEntitySystem.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.API {
    public class WeaponHealingGraphicEffectInputs : HealingGraphicEffectInputs {
        public WeaponHealingGraphicEffectInputs(Entity entity, Transform rotationTransform, SkinnedMeshRenderer renderer)
            : base(entity, renderer) => RotationTransform = rotationTransform;

        public Transform RotationTransform { get; }

        public override float TilingX => 2f;
    }
}