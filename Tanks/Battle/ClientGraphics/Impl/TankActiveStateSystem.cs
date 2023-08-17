using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class TankActiveStateSystem : ECSSystem {
        const float TRANSPARENCY_TRANSITION_TIME = 0.5f;

        [OnEventFire]
        public void InitTransitionFromSemiTransparent(NodeAddedEvent nodeAdded, TankActiveStateNode unit,
            [Context] [JoinByTank] WeaponNode weapon) {
            SetTransparencyToOpaque(unit.Entity);
            SetTransparencyToOpaque(weapon.Entity);
        }

        void SetTransparencyToOpaque(Entity entity) {
            TransparencyTransitionComponent transparencyTransitionComponent = new();
            transparencyTransitionComponent.TransparencyTransitionTime = 0.5f;
            transparencyTransitionComponent.OriginAlpha = ClientGraphicsConstants.SEMI_TRANSPARENT_ALPHA;
            transparencyTransitionComponent.TargetAlpha = ClientGraphicsConstants.OPAQUE_ALPHA;
            entity.AddComponent(transparencyTransitionComponent);
        }

        public class TankActiveStateNode : Node {
            public BaseRendererComponent baseRenderer;

            public TankActiveStateComponent tankActiveState;
            public TankGroupComponent tankGroup;
        }

        public class WeaponNode : Node {
            public BaseRendererComponent baseRenderer;
            public TankGroupComponent tankGroup;

            public WeaponComponent weapon;
        }
    }
}