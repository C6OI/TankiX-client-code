using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class ShaftAimingRendererSystem : ECSSystem {
        [OnEventFire]
        public void StartReducing(NodeAddedEvent evt, AimingWorkActivationNode weapon, [JoinByTank] TankNode tank,
            [JoinByTank] ICollection<RendererNode> renderers) {
            Shader transparentShader = tank.tankShader.transparentShader;
            SetTransparentMode(renderers, tank, transparentShader);

            if (weapon.Entity.HasComponent<ShaftAimingRendererRecoveringAlphaComponent>()) {
                weapon.Entity.RemoveComponent<ShaftAimingRendererRecoveringAlphaComponent>();
            }

            float alpha = TankMaterialsUtil.GetAlpha(tank.trackRenderer.Renderer);
            weapon.Entity.AddComponent(new ShaftAimingRendererReducingAlphaComponent(alpha));
            ShaftAimingRendererQueueMapComponent shaftAimingRendererQueueMapComponent = new();

            foreach (RendererNode renderer in renderers) {
                Material[] materials = renderer.baseRenderer.Renderer.materials;

                foreach (Material material in materials) {
                    shaftAimingRendererQueueMapComponent.QueueMap.Add(material, material.renderQueue);
                    material.renderQueue = weapon.shaftAimingRendererEffect.TransparentRenderQueue;
                }
            }

            weapon.Entity.AddComponent(shaftAimingRendererQueueMapComponent);
        }

        [OnEventFire]
        public void ProcessReducing(UpdateEvent evt, AimingWorkActivationAlphaTransitionNode weapon,
            [JoinByTank] TankNode tank, [JoinByTank] ICollection<RendererNode> renderers) {
            float t = weapon.shaftAimingWorkActivationState.ActivationTimer /
                      weapon.shaftStateConfig.ActivationToWorkingTransitionTimeSec;

            float num = Mathf.Lerp(weapon.shaftAimingRendererReducingAlpha.InitialAlpha,
                ClientGraphicsConstants.TRANSPARENT_ALPHA,
                t);

            float alpha = num;
            SetTransparentMode(renderers, tank, null, alpha);
        }

        [OnEventFire]
        public void SwitchAlphaMode(NodeAddedEvent evt, AimingIdleReducingNode weapon) {
            weapon.Entity.RemoveComponent<ShaftAimingRendererReducingAlphaComponent>();

            foreach (Material key in weapon.shaftAimingRendererQueueMap.QueueMap.Keys) {
                key.renderQueue = weapon.shaftAimingRendererQueueMap.QueueMap[key];
            }

            weapon.Entity.RemoveComponent<ShaftAimingRendererQueueMapComponent>();
            weapon.Entity.AddComponent<ShaftAimingRendererRecoveringAlphaComponent>();
        }

        [OnEventFire]
        public void SetOpaqueModeOnExitTankActiveState(NodeRemoveEvent evt, TankNode tank,
            [JoinByTank] ShaftAimingRendererEffectNode weapon, [JoinByTank] ICollection<RendererNode> renderers) {
            Shader opaqueShader = tank.tankShader.opaqueShader;
            SetTransparentMode(renderers, tank, opaqueShader, ClientGraphicsConstants.OPAQUE_ALPHA);
        }

        [OnEventFire]
        public void RecoverAlpha(TimeUpdateEvent evt, AimingRecoveringNode weapon, [JoinByTank] TankNode tank,
            [JoinByTank] ICollection<RendererNode> renderers) {
            float alphaRecoveringSpeed = weapon.shaftAimingRendererEffect.AlphaRecoveringSpeed;
            float alpha = TankMaterialsUtil.GetAlpha(tank.trackRenderer.Renderer);
            float num = alpha + alphaRecoveringSpeed * evt.DeltaTime;

            if (num >= ClientGraphicsConstants.OPAQUE_ALPHA) {
                Shader opaqueShader = tank.tankShader.opaqueShader;
                SetTransparentMode(renderers, tank, opaqueShader, ClientGraphicsConstants.OPAQUE_ALPHA);
                weapon.Entity.RemoveComponent<ShaftAimingRendererRecoveringAlphaComponent>();
            } else {
                float alpha2 = num;
                SetTransparentMode(renderers, tank, null, alpha2);
            }
        }

        void SetTransparentMode(ICollection<RendererNode> renderers, TankNode tank, Shader targetShader = null,
            float alpha = -1f) {
            foreach (RendererNode renderer in renderers) {
                if (targetShader != null) {
                    ClientGraphicsUtil.ApplyShaderToRenderer(renderer.baseRenderer.Renderer, targetShader);
                }

                if (alpha == 0f || tank.Entity.HasComponent<TankPartIntersectedWithCameraStateComponent>()) {
                    renderer.baseRenderer.Renderer.enabled = false;
                } else if (alpha > 0f) {
                    TankMaterialsUtil.SetAlpha(renderer.baseRenderer.Renderer, alpha);
                    renderer.baseRenderer.Renderer.enabled = true;
                }
            }
        }

        public class RendererNode : Node {
            public BaseRendererComponent baseRenderer;

            public TankGroupComponent tankGroup;

            public TankPartComponent tankPart;
        }

        public class TankNode : Node {
            public TankComponent tank;

            public TankActiveStateComponent tankActiveState;

            public TankGroupComponent tankGroup;
            public TankShaderComponent tankShader;

            public TrackRendererComponent trackRenderer;
        }

        public class ShaftAimingRendererEffectNode : Node {
            public ShaftAimingRendererEffectComponent shaftAimingRendererEffect;
            public ShaftStateControllerComponent shaftStateController;

            public TankGroupComponent tankGroup;
        }

        public class AimingWorkActivationNode : Node {
            public ShaftAimingRendererEffectComponent shaftAimingRendererEffect;
            public ShaftAimingWorkActivationStateComponent shaftAimingWorkActivationState;

            public ShaftStateControllerComponent shaftStateController;

            public TankGroupComponent tankGroup;
        }

        public class AimingIdleReducingNode : Node {
            public ShaftAimingRendererEffectComponent shaftAimingRendererEffect;

            public ShaftAimingRendererQueueMapComponent shaftAimingRendererQueueMap;

            public ShaftAimingRendererReducingAlphaComponent shaftAimingRendererReducingAlpha;
            public ShaftIdleStateComponent shaftIdleState;

            public ShaftStateControllerComponent shaftStateController;

            public TankGroupComponent tankGroup;
        }

        public class AimingRecoveringNode : Node {
            public ShaftAimingRendererEffectComponent shaftAimingRendererEffect;

            public ShaftAimingRendererRecoveringAlphaComponent shaftAimingRendererRecoveringAlpha;
            public ShaftStateControllerComponent shaftStateController;

            public TankGroupComponent tankGroup;
        }

        public class AimingWorkActivationAlphaTransitionNode : Node {
            public ShaftAimingRendererEffectComponent shaftAimingRendererEffect;

            public ShaftAimingRendererReducingAlphaComponent shaftAimingRendererReducingAlpha;
            public ShaftAimingWorkActivationStateComponent shaftAimingWorkActivationState;

            public ShaftStateConfigComponent shaftStateConfig;

            public ShaftStateControllerComponent shaftStateController;

            public TankGroupComponent tankGroup;
        }
    }
}