using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class TankActiveStateSystem : ECSSystem {
        const float TRANSPARENCY_TRANSITION_TIME = 0.5f;

        [OnEventFire]
        public void InitTransitionFromSemiTransparent(NodeAddedEvent nodeAdded, TankActiveStateNode unit, [Context] [JoinByTank] WeaponNode weapon) {
            SetTransparencyToOpaque(unit);
            SetTransparencyToOpaque(weapon);
        }

        void SetTransparencyToOpaque(RendererNode rendererNode) {
            Renderer renderer = rendererNode.baseRenderer.Renderer;
            renderer.materials = rendererNode.startMaterials.Materials;
            ScheduleEvent(new SetTransparencyTransitionDataEvent(ClientGraphicsConstants.SEMI_TRANSPARENT_ALPHA, ClientGraphicsConstants.OPAQUE_ALPHA, 0.5f), rendererNode);
        }

        public class RendererNode : Node {
            public BaseRendererComponent baseRenderer;

            public RendererPaintedComponent rendererPainted;

            public StartMaterialsComponent startMaterials;
            public TankGroupComponent tankGroup;
        }

        public class TankActiveStateNode : RendererNode {
            public TankActiveStateComponent tankActiveState;
        }

        public class WeaponNode : RendererNode {
            public WeaponComponent weapon;
        }
    }
}