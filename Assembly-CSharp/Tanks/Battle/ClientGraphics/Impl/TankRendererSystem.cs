using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;
using UnityEngine.Rendering;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class TankRendererSystem : ECSSystem {
        [OnEventFire]
        public void HideAndPrepareRenderer(NodeAddedEvent evt, TankPartRendererNode renderer) {
            Renderer renderer2 = renderer.baseRenderer.Renderer;
            renderer2.enabled = false;
            TankMaterialsUtil.SetAlpha(renderer2, ClientGraphicsConstants.OPAQUE_ALPHA);
            renderer.startMaterials.Materials = renderer2.materials;
            renderer.Entity.AddComponent<RendererInitializedComponent>();
        }

        [OnEventFire]
        public void SetColoringTextureToRenderer(NodeAddedEvent evt, AssembledTankNode tank, [JoinByTank] [Context] TankRendererInitializedNode tankRendererNode,
            [JoinByTank] [Context] WeaponRendererInitializedNode weaponRendererNode) {
            TankMaterialsUtil.ApplyColoring(tankRendererNode.baseRenderer.Renderer, weaponRendererNode.baseRenderer.Renderer, tankRendererNode.coloring, weaponRendererNode.coloring);
            tankRendererNode.Entity.AddComponent<RendererPaintedComponent>();
            weaponRendererNode.Entity.AddComponent<RendererPaintedComponent>();
            tankRendererNode.coloring.transform.localPosition = Vector3.zero;
            weaponRendererNode.coloring.transform.localPosition = Vector3.zero;

            if (tankRendererNode.coloring.overrideEmission) {
                tankRendererNode.doubleArmorEffect.usualEmissionColor = tankRendererNode.coloring.emissionColor;
            }

            if (weaponRendererNode.coloring.overrideEmission) {
                weaponRendererNode.doubleDamageEffect.usualEmissionColor = weaponRendererNode.coloring.emissionColor;
            }
        }

        [OnEventFire]
        public void SetStartMaterials(NodeAddedEvent e, TankIncarnationNode incarnation, [Combine] [Context] [JoinByTank] RendererReadyForShowingNode renderer) {
            SetStartMaterials(renderer);
        }

        void SetStartMaterials(RendererReadyForShowingNode renderer) {
            Renderer renderer2 = renderer.baseRenderer.Renderer;
            renderer2.materials = renderer.startMaterials.Materials;
            ScheduleEvent<TransparencyFinalizeEvent>(renderer);
        }

        [OnEventFire]
        public void ShowRenderersVisibleState(NodeAddedEvent evt, [Combine] RendererReadyForShowingNode renderer, [Context] [JoinByTank] AssembledTankNode tank,
            [Context] [JoinByTank] VisibleTankNode state) {
            Renderer renderer2 = renderer.baseRenderer.Renderer;
            renderer2.enabled = true;
        }

        [OnEventFire]
        public void HideRenderersVisibleState(NodeRemoveEvent evt, [Combine] RendererReadyForShowingNode renderer, [Context] [JoinByTank] AssembledTankNode tank,
            [Context] [JoinByTank] VisibleTankNode state) {
            Renderer renderer2 = renderer.baseRenderer.Renderer;
            renderer2.enabled = false;
        }

        [OnEventFire]
        public void DisableShadowOnDeadState(NodeRemoveEvent evt, DeadTankNode state, [Combine] [JoinByTank] TankPartRendererNode renderer) {
            Renderer renderer2 = renderer.baseRenderer.Renderer;
            renderer2.enabled = false;
            TankMaterialsUtil.SetAlpha(renderer2, 0f);
        }

        [OnEventFire]
        public void SetShadowOnIntersectionWithCamera(NodeAddedEvent evt, IntersectedRenderer renderer) {
            renderer.baseRenderer.Renderer.shadowCastingMode = ShadowCastingMode.Off;
            renderer.baseRenderer.Renderer.receiveShadows = false;
        }

        [OnEventFire]
        public void SetShadowOnIntersectionWithCamera(NodeAddedEvent evt, NotIntersectedRenderer renderer) {
            renderer.baseRenderer.Renderer.shadowCastingMode = ShadowCastingMode.On;
            renderer.baseRenderer.Renderer.receiveShadows = true;
        }

        public class TankPartRendererNode : Node {
            public BaseRendererComponent baseRenderer;

            public StartMaterialsComponent startMaterials;
            public TankGroupComponent tankGroup;

            public TankPartComponent tankPart;
        }

        public class RendererInitializedNode : TankPartRendererNode {
            public RendererInitializedComponent rendererInitialized;
        }

        public class TankRendererInitializedNode : RendererInitializedNode {
            public ColoringComponent coloring;

            public DoubleArmorEffectComponent doubleArmorEffect;
            public TankComponent tank;
        }

        public class WeaponRendererInitializedNode : RendererInitializedNode {
            public ColoringComponent coloring;

            public DoubleDamageEffectComponent doubleDamageEffect;
            public WeaponComponent weapon;
        }

        public class RendererReadyForShowingNode : RendererInitializedNode {
            public RendererPaintedComponent rendererPainted;
        }

        public class TankIncarnationNode : Node {
            public TankGroupComponent tankGroup;
            public TankIncarnationComponent tankIncarnation;
        }

        public class TankNode : Node {
            public TankComponent tank;
            public TankGroupComponent tankGroup;
        }

        public class AssembledTankNode : TankNode {
            public AssembledTankActivatedStateComponent assembledTankActivatedState;
        }

        public class DeadTankNode : TankNode {
            public TankDeadStateComponent tankDeadState;
        }

        public class VisibleTankNode : TankNode {
            public TankVisibleStateComponent tankVisibleState;
        }

        public class IntersectedRenderer : TankPartRendererNode {
            public TankPartIntersectedWithCameraStateComponent tankPartIntersectedWithCameraState;
        }

        public class NotIntersectedRenderer : TankPartRendererNode {
            public TankPartNotIntersectedWithCameraStateComponent tankPartNotIntersectedWithCameraState;
        }
    }
}