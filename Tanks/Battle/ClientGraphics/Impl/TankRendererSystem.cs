using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;
using UnityEngine.Rendering;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class TankRendererSystem : ECSSystem {
        [OnEventFire]
        public void HideRenderer(NodeAddedEvent evt, InitialRendererNode renderer) {
            Renderer renderer2 = renderer.baseRenderer.Renderer;
            renderer2.enabled = false;
            TankMaterialsUtil.SetAlpha(renderer2, ClientGraphicsConstants.OPAQUE_ALPHA);
            renderer.Entity.AddComponent<RendererAlphaInitializedComponent>();
        }

        [OnEventFire]
        public void SetColoringTextureToRenderer(NodeAddedEvent evt, [Combine] RendererAlphaInitializedNode rendererNode,
            [JoinByTank] [Context] AssembledTankNode tank) {
            Texture2D tankActiveTexture = tank.tankActiveTextureBehaviour.tankActiveTexture;
            Renderer renderer = rendererNode.baseRenderer.Renderer;
            TankMaterialsUtil.SetColoringTexture(renderer, tankActiveTexture);
            rendererNode.Entity.AddComponent<RendererPaintedComponent>();
        }

        [OnEventFire]
        public void ShowRenderersOnActiveState(NodeAddedEvent evt, [Combine] RendererReadyForShowingNode renderer,
            [Context] [JoinByTank] AssembledTankNode tank, [Context] [JoinByTank] ActiveTankNode state) {
            Renderer renderer2 = renderer.baseRenderer.Renderer;
            renderer2.enabled = true;
        }

        [OnEventFire]
        public void ShowRenderersOnSemiActiveState(NodeAddedEvent evt, [Combine] RendererReadyForShowingNode renderer,
            [JoinByTank] [Context] AssembledTankNode tank, [JoinByTank] [Context] SemiActiveTankNode state) {
            Renderer renderer2 = renderer.baseRenderer.Renderer;
            renderer2.enabled = true;
        }

        [OnEventFire]
        public void ShowRenderersOnDeadState(NodeAddedEvent evt, [Combine] RendererReadyForShowingNode renderer,
            [Context] [JoinByTank] AssembledTankNode tank, [JoinByTank] [Context] DeadTankNode state) {
            Renderer renderer2 = renderer.baseRenderer.Renderer;
            renderer2.enabled = true;
        }

        [OnEventFire]
        public void DisableShadowOnDeadState(NodeRemoveEvent evt, DeadTankNode state,
            [Combine] [JoinByTank] TankPartRendererNode renderer) {
            Renderer renderer2 = renderer.baseRenderer.Renderer;
            renderer2.enabled = false;
            renderer.baseRenderer.Renderer.shadowCastingMode = ShadowCastingMode.Off;
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

        public class InitialRendererNode : Node {
            public BaseRendererComponent baseRenderer;
            public TankGroupComponent tankGroup;

            public TankPartComponent tankPart;
        }

        public class RendererAlphaInitializedNode : Node {
            public BaseRendererComponent baseRenderer;

            public RendererAlphaInitializedComponent rendererAlphaInitialized;
            public TankGroupComponent tankGroup;

            public TankPartComponent tankPart;
        }

        public class RendererReadyForShowingNode : Node {
            public BaseRendererComponent baseRenderer;

            public RendererAlphaInitializedComponent rendererAlphaInitialized;

            public RendererPaintedComponent rendererPainted;
            public TankGroupComponent tankGroup;

            public TankPartComponent tankPart;
        }

        public class TankPartRendererNode : Node {
            public BaseRendererComponent baseRenderer;
            public TankGroupComponent tankGroup;

            public TankPartComponent tankPart;
        }

        public class AssembledTankNode : Node {
            public AssembledTankActivatedStateComponent assembledTankActivatedState;

            public TankActiveTextureBehaviourComponent tankActiveTextureBehaviour;
            public TankGroupComponent tankGroup;
        }

        public class SemiActiveTankNode : Node {
            public TankGroupComponent tankGroup;

            public TankSemiActiveStateComponent tankSemiActiveState;
        }

        public class ActiveTankNode : Node {
            public TankActiveStateComponent tankActiveState;
            public TankGroupComponent tankGroup;
        }

        public class DeadTankNode : Node {
            public TankDeadStateComponent tankDeadState;
            public TankGroupComponent tankGroup;
        }

        public class IntersectedRenderer : Node {
            public BaseRendererComponent baseRenderer;
            public TankPartIntersectedWithCameraStateComponent tankPartIntersectedWithCameraState;
        }

        public class NotIntersectedRenderer : Node {
            public BaseRendererComponent baseRenderer;
            public TankPartNotIntersectedWithCameraStateComponent tankPartNotIntersectedWithCameraState;
        }
    }
}