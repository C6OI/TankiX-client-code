using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class CommonWeaponVisualBuilderSystem : ECSSystem {
        [OnEventFire]
        public void BuildVisualWeapon(NodeAddedEvent evt, WeaponGraphicsNode weaponGraphics) {
            Entity entity = weaponGraphics.Entity;
            BaseRendererComponent baseRendererComponent = new();

            Renderer renderer = baseRendererComponent.Renderer =
                                    TankBuilderUtil.GetWeaponRenderer(weaponGraphics.weaponVisualRoot.gameObject);

            baseRendererComponent.Mesh = (baseRendererComponent.Renderer as SkinnedMeshRenderer).sharedMesh;
            entity.AddComponent(baseRendererComponent);
            Bounds bounds = renderer.bounds;
            WeaponBoundsComponent weaponBoundsComponent = new();
            weaponBoundsComponent.WeaponBounds = bounds;
            entity.AddComponent(weaponBoundsComponent);
        }

        public class WeaponGraphicsNode : Node {
            public TankGroupComponent tankGroup;
            public WeaponVisualRootComponent weaponVisualRoot;
        }
    }
}