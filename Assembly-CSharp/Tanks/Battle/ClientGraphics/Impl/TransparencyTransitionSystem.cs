using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class TransparencyTransitionSystem : ECSSystem {
        [OnEventFire]
        public void SetTransparencyTransitionData(SetTransparencyTransitionDataEvent evt, NotTransitionRendererNode renderer) {
            TransparencyTransitionComponent transparencyTransitionComponent = new();
            SetTransparencyTransitionData(evt, renderer, transparencyTransitionComponent);
            renderer.Entity.AddComponent(transparencyTransitionComponent);
            ScheduleEvent<TransparencyInitEvent>(renderer.Entity);
        }

        [OnEventFire]
        public void SetTransparencyTransitionData(SetTransparencyTransitionDataEvent evt, TransitionRendererNode renderer) {
            SetTransparencyTransitionData(evt, renderer, renderer.transparencyTransition);
            ScheduleEvent<TransparencyInitEvent>(renderer.Entity);
        }

        void SetTransparencyTransitionData(SetTransparencyTransitionDataEvent evt, RendererNode renderer, TransparencyTransitionComponent transparencyTransition) {
            transparencyTransition.OriginAlpha = evt.OriginAlpha;
            transparencyTransition.TargetAlpha = evt.TargetAlpha;
            transparencyTransition.TransparencyTransitionTime = evt.TransparencyTransitionTime;
            renderer.baseRenderer.Renderer.enabled = true;
            transparencyTransition.AlphaSpeed = (transparencyTransition.TargetAlpha - transparencyTransition.OriginAlpha) / transparencyTransition.TransparencyTransitionTime;
            transparencyTransition.CurrentAlpha = transparencyTransition.OriginAlpha;
        }

        [OnEventFire]
        public void UpdateTransparencyTransition(TimeUpdateEvent evt, TransitionRendererNode renderer) {
            TransparencyTransitionComponent transparencyTransition = renderer.transparencyTransition;
            transparencyTransition.CurrentTransitionTime += evt.DeltaTime;

            if (!renderer.baseRenderer.Renderer) {
                return;
            }

            float num;

            if (transparencyTransition.CurrentTransitionTime >= transparencyTransition.TransparencyTransitionTime) {
                num = transparencyTransition.TargetAlpha;

                if (transparencyTransition.TargetAlpha >= ClientGraphicsConstants.OPAQUE_ALPHA) {
                    ScheduleEvent<TransparencyFinalizeEvent>(renderer.Entity);
                } else if (transparencyTransition.TargetAlpha <= ClientGraphicsConstants.TRANSPARENT_ALPHA) {
                    renderer.baseRenderer.Renderer.enabled = false;
                }
            } else {
                num = transparencyTransition.OriginAlpha + transparencyTransition.AlphaSpeed * transparencyTransition.CurrentTransitionTime;
            }

            renderer.transparencyTransition.CurrentAlpha = num;
            Renderer renderer2 = renderer.baseRenderer.Renderer;
            TankMaterialsUtil.SetAlpha(renderer2, num);
        }

        [OnEventComplete]
        public void InitTransparency(TransparencyInitEvent evt, RendererNode renderer, [JoinByTank] TankNotInvisibilityInvisibilityShaderNode tankShader) {
            ClientGraphicsUtil.ApplyShaderToRenderer(renderer.baseRenderer.Renderer, tankShader.tankShader.TransparentShader);
        }

        [OnEventComplete]
        public void FinalizeTransparency(TransparencyFinalizeEvent evt, TransitionRendererNode renderer, [JoinByTank] TankShaderNode tankShader) {
            renderer.Entity.RemoveComponent<TransparencyTransitionComponent>();
            TankMaterialsUtil.SetAlpha(renderer.baseRenderer.Renderer, ClientGraphicsConstants.OPAQUE_ALPHA);
            ScheduleEvent(new StopTankShaderEffectEvent(ClientGraphicsConstants.TRANSPARENCY_TRANSITION_EFFECT, false), tankShader);
        }

        public class RendererNode : Node {
            public BaseRendererComponent baseRenderer;
            public TankGroupComponent tankGroup;
        }

        public class TransitionRendererNode : RendererNode {
            public TransparencyTransitionComponent transparencyTransition;
        }

        [Not(typeof(TransparencyTransitionComponent))]
        public class NotTransitionRendererNode : RendererNode { }

        public class TankShaderNode : Node {
            public TankGroupComponent tankGroup;

            public TankOpaqueShaderBlockersComponent tankOpaqueShaderBlockers;

            public TankShaderComponent tankShader;
        }

        [Not(typeof(TankInvisibilityEffectActivationStateComponent))]
        [Not(typeof(TankInvisibilityEffectWorkingStateComponent))]
        [Not(typeof(TankInvisibilityEffectDeactivationStateComponent))]
        public class TankNotInvisibilityInvisibilityShaderNode : TankShaderNode { }
    }
}