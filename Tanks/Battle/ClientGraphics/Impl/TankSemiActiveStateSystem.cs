using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientGraphics.API;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class TankSemiActiveStateSystem : ECSSystem {
        [OnEventFire]
        public void InitSemiActiveTransition(NodeAddedEvent nodeAdded, SemiActiveTankNode tank,
            [Combine] [Context] [JoinByTank] RendererNode renderer) {
            ScheduleEvent<TransparencyInitEvent>(renderer);
            TankMaterialsUtil.SetAlpha(renderer.baseRenderer.Renderer, ClientGraphicsConstants.SEMI_TRANSPARENT_ALPHA);
        }

        public class SemiActiveTankNode : Node {
            public TankGroupComponent tankGroup;

            public TankSemiActiveStateComponent tankSemiActiveState;

            public TankShaderComponent tankShader;
        }

        public class RendererNode : Node {
            public BaseRendererComponent baseRenderer;

            public RendererAlphaInitializedComponent rendererAlphaInitialized;
            public TankGroupComponent tankGroup;

            public TankPartComponent tankPart;
        }
    }
}