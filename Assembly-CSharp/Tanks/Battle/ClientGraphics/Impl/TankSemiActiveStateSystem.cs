using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientGraphics.API;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class TankSemiActiveStateSystem : ECSSystem {
        [OnEventFire]
        public void InitSemiActiveTransition(NodeAddedEvent e, SemiActiveTankNode tank, [Combine] [Context] [JoinByTank] RendererNode renderer) {
            ScheduleEvent(new AddTankShaderEffectEvent(ClientGraphicsConstants.TRANSPARENCY_TRANSITION_EFFECT), tank);
            ScheduleEvent<TransparencyInitEvent>(renderer);
            TankMaterialsUtil.SetAlpha(renderer.baseRenderer.Renderer, ClientGraphicsConstants.SEMI_TRANSPARENT_ALPHA);
        }

        [OnEventFire]
        public void CloseSemiActiveTransition(NodeRemoveEvent e, SemiActiveTankNode tank, [Combine] [JoinByTank] SingleNode<BaseRendererComponent> renderer) {
            ScheduleEvent<TransparencyFinalizeEvent>(renderer);
        }

        public class SemiActiveTankNode : Node {
            public TankGroupComponent tankGroup;

            public TankOpaqueShaderBlockersComponent tankOpaqueShaderBlockers;

            public TankSemiActiveStateComponent tankSemiActiveState;

            public TankShaderComponent tankShader;
        }

        public class RendererNode : Node {
            public BaseRendererComponent baseRenderer;

            public RendererPaintedComponent rendererPainted;
            public TankGroupComponent tankGroup;

            public TankPartComponent tankPart;
        }
    }
}