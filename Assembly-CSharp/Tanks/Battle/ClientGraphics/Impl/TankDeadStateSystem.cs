using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class TankDeadStateSystem : ECSSystem {
        const float ALPHA_TRANSITION_TIME = 0.5f;

        [OnEventFire]
        public void SetFadeStartTime(NodeAddedEvent evt, TankDeadStateNode tank) {
            tank.tankDeadStateTexture.FadeStart = Date.Now;
        }

        [OnEventFire]
        public void UpdateMaterialsForDeath(NodeAddedEvent e, TankDeadStateNode tank, [Combine] [Context] [JoinByTank] RendererNode renderer) {
            ScheduleEvent<TransparencyFinalizeEvent>(renderer);
            SwitchToDeathMaterials(renderer);
        }

        void SwitchToDeathMaterials(RendererNode renderer) {
            Renderer renderer2 = renderer.baseRenderer.Renderer;
            renderer2.materials = renderer.tankPartMaterialForDeath.DeathMaterials;
        }

        [OnEventComplete]
        public void FadeTemperature(UpdateEvent e, TankDeadStateVisibleActivatedNode tank, [JoinByTank] ICollection<RendererNode> renderers) {
            float num = (tank.tankDeadState.EndDate - Date.Now) / (tank.tankDeadState.EndDate - tank.tankDeadStateTexture.FadeStart);

            if (num == tank.tankDeadStateTexture.LastFade) {
                return;
            }

            tank.tankDeadStateTexture.LastFade = num;

            Vector4 vector = new(tank.tankDeadStateTexture.HeatEmission.Evaluate(num), 0f, tank.tankDeadStateTexture.WhiteToHeatTexture.Evaluate(num),
                tank.tankDeadStateTexture.PaintTextureToWhiteHeat.Evaluate(num));

            foreach (RendererNode renderer in renderers) {
                ClientGraphicsUtil.UpdateVector(renderer.baseRenderer.Renderer, ClientGraphicsConstants.TEMPERATURE, vector);
            }
        }

        [OnEventComplete]
        public void StartBeingTranparent(UpdateEvent e, TankDeadStateVisibleActivatedNode tank, [JoinByTank] ICollection<OpaqueRendererNode> renderers) {
            if (!(Date.Now.AddSeconds(0.5f) >= tank.tankDeadState.EndDate)) {
                return;
            }

            foreach (OpaqueRendererNode renderer in renderers) {
                renderer.Entity.AddComponent<TransparentComponent>();
                ScheduleEvent(new SetTransparencyTransitionDataEvent(ClientGraphicsConstants.OPAQUE_ALPHA, ClientGraphicsConstants.TRANSPARENT_ALPHA, 0.5f), renderer);
            }
        }

        [OnEventComplete]
        public void StopBeingTranparent(NodeRemoveEvent e, TankDeadStateVisibleActivatedNode tank, [JoinByTank] ICollection<TransparnetRendererNode> renderers) {
            foreach (TransparnetRendererNode renderer in renderers) {
                renderer.Entity.RemoveComponent<TransparentComponent>();
            }
        }

        public class TankDeadStateNode : Node {
            public TankDeadStateComponent tankDeadState;

            public TankDeadStateTextureComponent tankDeadStateTexture;
            public TankGroupComponent tankGroup;

            public TankShaderComponent tankShader;
        }

        public class TankDeadStateVisibleActivatedNode : TankDeadStateNode {
            public AssembledTankActivatedStateComponent assembledTankActivatedState;
        }

        [Not(typeof(BrokenEffectComponent))]
        public class RendererNode : Node {
            public BaseRendererComponent baseRenderer;

            public RendererPaintedComponent rendererPainted;
            public TankGroupComponent tankGroup;

            public TankPartMaterialForDeathComponent tankPartMaterialForDeath;
        }

        [Not(typeof(TransparentComponent))]
        public class OpaqueRendererNode : RendererNode { }

        public class TransparnetRendererNode : RendererNode {
            public TransparentComponent transparent;
        }
    }
}