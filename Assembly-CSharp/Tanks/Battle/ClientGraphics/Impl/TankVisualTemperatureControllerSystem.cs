using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class TankVisualTemperatureControllerSystem : ECSSystem {
        [OnEventFire]
        public void AddTargetToBurningBloomPostEffect(NodeAddedEvent e, SingleNode<BurningTargetBloomComponent> bloomPostEffect, [Combine] RendererNode rendererNode,
            [JoinByTank] [Context] SingleNode<TankActiveStateComponent> tank) {
            bloomPostEffect.component.burningTargetBloom.targets.Add(rendererNode.baseRenderer.Renderer);
        }

        [OnEventFire]
        public void RemoveTargetFromBurningBloomPostEffect(NodeRemoveEvent e, SingleNode<TankActiveStateComponent> tank, [JoinByTank] ICollection<RendererNode> rendererNodes,
            [JoinAll] SingleNode<BurningTargetBloomComponent> bloomPostEffect) {
            foreach (RendererNode rendererNode in rendererNodes) {
                bloomPostEffect.component.burningTargetBloom.targets.Remove(rendererNode.baseRenderer.Renderer);
            }
        }

        [OnEventFire]
        public void SetTemperature(NodeAddedEvent evt, TemperatureEffectNode node, [Combine] [JoinByTank] SingleNode<TemperatureVisualControllerComponent> visualController) {
            visualController.component.Temperature = node.logicToVisualTemperatureConverter.ConvertToVisualTemperature(node.temperature.Temperature);
        }

        [OnEventFire]
        public void UpdateTemperature(UpdateEvent evt, TemperatureEffectNode node, [Combine] [JoinByTank] SingleNode<TemperatureVisualControllerComponent> visualController) {
            visualController.component.Temperature = Mathf.Lerp(visualController.component.Temperature,
                node.logicToVisualTemperatureConverter.ConvertToVisualTemperature(node.temperature.Temperature), evt.DeltaTime);
        }

        [OnEventFire]
        public void ResetTemperature(NodeRemoveEvent evt, TemperatureDeadEffectNode node, [Combine] [JoinByTank] SingleNode<TemperatureVisualControllerComponent> visualController) {
            visualController.component.Temperature = 0f;
            visualController.component.Reset();
        }

        [OnEventFire]
        public void ResetTemperature(NodeAddedEvent evt, TemperatureSemiActiveEffectNode node, [Combine] [JoinByTank] SingleNode<TemperatureVisualControllerComponent> visualController) {
            visualController.component.Temperature = 0f;
            visualController.component.Reset();
        }

        public class TemperatureEffectNode : Node {
            public LogicToVisualTemperatureConverterComponent logicToVisualTemperatureConverter;
            public TankActiveStateComponent tankActiveState;

            public TemperatureComponent temperature;
        }

        public class TemperatureDeadEffectNode : Node {
            public LogicToVisualTemperatureConverterComponent logicToVisualTemperatureConverter;
            public TankDeadStateComponent tankDeadState;

            public TemperatureComponent temperature;
        }

        public class TemperatureSemiActiveEffectNode : Node {
            public LogicToVisualTemperatureConverterComponent logicToVisualTemperatureConverter;
            public TankSemiActiveStateComponent tankSemiActiveState;

            public TemperatureComponent temperature;
        }

        public class RendererNode : Node {
            public BaseRendererComponent baseRenderer;

            public TankGroupComponent tankGroup;
        }
    }
}