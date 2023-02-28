using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class ModuleVisualEffectObjectsComponent : Component {
        public GameObject ExplosiveMassEffect;

        public GameObject ExternalImpactEffect;

        public GameObject FireRingEffect;
        public GameObject JumpImpactEffect;
    }
}