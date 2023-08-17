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
        public void SetFadeStartTime(NodeAddedEvent evt, TankDeadStateNode tank) =>
            tank.tankDeadStateTexture.FadeStart = Date.Now;

        [OnEventFire]
        public void UpdateMaterialsForDeath(NodeAddedEvent e, TankDeadStateNode tank,
            [JoinByTank] [Context] [Combine] RendererNode renderer) {
            ScheduleEvent<TransparencyFinalizeEvent>(renderer);
            Texture2D deadColorTexture = tank.tankDeadStateTexture.DeadColorTexture;
            Texture2D deadEmissionTexture = tank.tankDeadStateTexture.DeadEmissionTexture;
            SwitchToOldMaterials(renderer, deadColorTexture, deadEmissionTexture);
            tank.tankShader.transparentShader = tank.tankShader.deadTransparentShader;
        }

        void SwitchToOldMaterials(RendererNode renderer, Texture2D deadColorTexture, Texture2D deadEmissionTexture) {
            Renderer renderer2 = renderer.baseRenderer.Renderer;
            renderer2.materials = renderer.tankPartMaterialForDeath.DeathMaterials;
            Material[] materials = renderer2.materials;
            int num = materials.Length;

            for (int i = 0; i < num; i++) {
                Material mat = materials[i];
                UpdateMaterialProperties(mat, deadColorTexture, deadEmissionTexture);
            }
        }

        void UpdateMaterialProperties(Material mat, Texture2D deadColorTexture, Texture2D deadEmissionTexture) {
            mat.SetTexture(ClientGraphicsConstants.PAINTMAP_PROPERTY, deadColorTexture);
            mat.SetTexture(ClientGraphicsConstants.EFFECT_MASK, deadEmissionTexture);
            mat.SetVector(ClientGraphicsConstants.TEMPERATURE, new Vector4(1f, 0f, 1f, 1f));
        }

        [OnEventComplete]
        public void FadeTemperature(UpdateEvent e, TankDeadStateVisibleActivatedNode tank,
            [JoinByTank] ICollection<RendererNode> renderers) {
            float time = (tank.tankDeadState.EndDate - Date.Now) /
                         (tank.tankDeadState.EndDate - tank.tankDeadStateTexture.FadeStart);

            Vector4 vector = new(tank.tankDeadStateTexture.HeatEmission.Evaluate(time),
                0f,
                tank.tankDeadStateTexture.WhiteToHeatTexture.Evaluate(time),
                tank.tankDeadStateTexture.PaintTextureToWhiteHeat.Evaluate(time));

            foreach (RendererNode renderer in renderers) {
                ClientGraphicsUtil.UpdateVector(renderer.baseRenderer.Renderer, ClientGraphicsConstants.TEMPERATURE, vector);
            }
        }

        [OnEventComplete]
        public void StartBeingTranparent(UpdateEvent e, TankDeadStateActivatedNode tank,
            [JoinByTank] ICollection<OpaqueRendererNode> renderers) {
            if (!(Date.Now.AddSeconds(0.5f) >= tank.tankDeadState.EndDate)) {
                return;
            }

            foreach (OpaqueRendererNode renderer in renderers) {
                renderer.Entity.AddComponent<TransparentComponent>();
                TransparencyTransitionComponent transparencyTransitionComponent = new();
                transparencyTransitionComponent.TransparencyTransitionTime = 0.5f;
                transparencyTransitionComponent.OriginAlpha = ClientGraphicsConstants.OPAQUE_ALPHA;
                transparencyTransitionComponent.TargetAlpha = ClientGraphicsConstants.TRANSPARENT_ALPHA;
                renderer.Entity.AddComponent(transparencyTransitionComponent);
            }
        }

        public class TankDeadStateNode : Node {
            public TankDeadStateComponent tankDeadState;

            public TankDeadStateTextureComponent tankDeadStateTexture;
            public TankGroupComponent tankGroup;

            public TankShaderComponent tankShader;
        }

        public class TankDeadStateActivatedNode : Node {
            public AssembledTankActivatedStateComponent assembledTankActivatedState;

            public TankDeadStateComponent tankDeadState;

            public TankDeadStateTextureComponent tankDeadStateTexture;
            public TankGroupComponent tankGroup;
        }

        public class TankDeadStateVisibleActivatedNode : Node {
            public AssembledTankActivatedStateComponent assembledTankActivatedState;

            public TankDeadStateComponent tankDeadState;

            public TankDeadStateTextureComponent tankDeadStateTexture;
            public TankGroupComponent tankGroup;
        }

        public class RendererNode : Node {
            public BaseRendererComponent baseRenderer;

            public RendererPaintedComponent rendererPainted;
            public TankGroupComponent tankGroup;

            public TankPartMaterialForDeathComponent tankPartMaterialForDeath;
        }

        [Not(typeof(TransparentComponent))]
        public class OpaqueRendererNode : Node {
            public BaseRendererComponent baseRenderer;

            public RendererPaintedComponent rendererPainted;
            public TankGroupComponent tankGroup;
        }
    }
}