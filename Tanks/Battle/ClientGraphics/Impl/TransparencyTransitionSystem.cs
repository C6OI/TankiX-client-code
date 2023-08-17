using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class TransparencyTransitionSystem : ECSSystem {
        [OnEventFire]
        public void InitTransparency(NodeAddedEvent evt, TransitionRendererNode renderer) {
            renderer.baseRenderer.Renderer.enabled = true;
            TransparencyTransitionComponent transparencyTransition = renderer.transparencyTransition;

            transparencyTransition.AlphaSpeed = (transparencyTransition.TargetAlpha - transparencyTransition.OriginAlpha) /
                                                transparencyTransition.TransparencyTransitionTime;

            renderer.transparencyTransition.CurrentAlpha = renderer.transparencyTransition.OriginAlpha;
            ScheduleEvent<TransparencyInitEvent>(renderer.Entity);
        }

        [OnEventFire]
        public void UpdateTransparencyTransition(TimeUpdateEvent evt, TransitionRendererNode renderer) {
            TransparencyTransitionComponent transparencyTransition = renderer.transparencyTransition;
            transparencyTransition.CurrentTransitionTime += evt.DeltaTime;
            float num;

            if (transparencyTransition.CurrentTransitionTime >= transparencyTransition.TransparencyTransitionTime) {
                num = transparencyTransition.TargetAlpha;

                if (transparencyTransition.TargetAlpha >= ClientGraphicsConstants.OPAQUE_ALPHA) {
                    ScheduleEvent<TransparencyFinalizeEvent>(renderer.Entity);
                } else if (transparencyTransition.TargetAlpha <= ClientGraphicsConstants.TRANSPARENT_ALPHA) {
                    renderer.baseRenderer.Renderer.enabled = false;
                }
            } else {
                num = transparencyTransition.OriginAlpha +
                      transparencyTransition.AlphaSpeed * transparencyTransition.CurrentTransitionTime;
            }

            renderer.transparencyTransition.CurrentAlpha = num;
            Renderer renderer2 = renderer.baseRenderer.Renderer;
            TankMaterialsUtil.SetAlpha(renderer2, num);
        }

        [OnEventComplete]
        public void InitTransparency(TransparencyInitEvent evt, RendererNode renderer,
            [JoinByTank] TankShaderNode tankShader) =>
            ClientGraphicsUtil.ApplyShaderToRenderer(renderer.baseRenderer.Renderer,
                tankShader.tankShader.transparentShader);

        [OnEventComplete]
        public void FinalizeTransparency(TransparencyFinalizeEvent evt, TransitionRendererNode renderer,
            [JoinByTank] TankShaderNode tankShader) {
            renderer.Entity.RemoveComponent<TransparencyTransitionComponent>();
            ClientGraphicsUtil.ApplyShaderToRenderer(renderer.baseRenderer.Renderer, tankShader.tankShader.opaqueShader);
        }

        public class RendererNode : Node {
            public BaseRendererComponent baseRenderer;
            public TankGroupComponent tankGroup;
        }

        public class TransitionRendererNode : RendererNode {
            public TransparencyTransitionComponent transparencyTransition;
        }

        public class TankShaderNode : Node {
            public TankGroupComponent tankGroup;

            public TankShaderComponent tankShader;
        }
    }
}