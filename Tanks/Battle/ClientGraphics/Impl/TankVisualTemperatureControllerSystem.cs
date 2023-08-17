using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class TankVisualTemperatureControllerSystem : ECSSystem {
        [OnEventFire]
        public void AddTargetToBurningBloomPostEffect(NodeAddedEvent e,
            SingleNode<BurningTargetBloomComponent> bloomPostEffect, [Combine] RendererNode rendererNode,
            [Context] [JoinByTank] SingleNode<TankActiveStateComponent> tank) =>
            bloomPostEffect.component.burningTargetBloom.targets.Add(rendererNode.baseRenderer.Renderer);

        [OnEventFire]
        public void RemoveTargetFromBurningBloomPostEffect(NodeRemoveEvent e, SingleNode<TankActiveStateComponent> tank,
            [JoinByTank] ICollection<RendererNode> rendererNodes,
            [JoinAll] SingleNode<BurningTargetBloomComponent> bloomPostEffect) {
            foreach (RendererNode rendererNode in rendererNodes) {
                bloomPostEffect.component.burningTargetBloom.targets.Remove(rendererNode.baseRenderer.Renderer);
            }
        }

        [OnEventFire]
        public void SetTemperature(NodeAddedEvent evt, TemperatureEffectNode node,
            [JoinByTank] [Combine] SingleNode<TemperatureVisualControllerComponent> visualController) =>
            visualController.component.Temperature =
                node.logicToVisualTemperatureConverter.ConvertToVisualTemperature(node.temperature.Temperature);

        [OnEventFire]
        public void UpdateTemperature(UpdateEvent evt, TemperatureEffectNode node,
            [JoinByTank] [Combine] SingleNode<TemperatureVisualControllerComponent> visualController) =>
            visualController.component.Temperature = Mathf.Lerp(visualController.component.Temperature,
                node.logicToVisualTemperatureConverter.ConvertToVisualTemperature(node.temperature.Temperature),
                evt.DeltaTime);

        [OnEventFire]
        public void ResetTemperature(NodeRemoveEvent evt, TemperatureEffectNode node,
            [JoinByTank] [Combine] SingleNode<TemperatureVisualControllerComponent> visualController) =>
            visualController.component.Temperature = 0f;

        public class TemperatureEffectNode : Node {
            public LogicToVisualTemperatureConverterComponent logicToVisualTemperatureConverter;
            public TankActiveStateComponent tankActiveState;

            public TemperatureComponent temperature;
        }

        public class RendererNode : Node {
            public BaseRendererComponent baseRenderer;

            public TankGroupComponent tankGroup;
        }
    }
}