using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class SupplyPropertyBuilderSystem : ECSSystem {
        [OnEventFire]
        public void CreatePropertyEntity(NodeAddedEvent e, SupplyNode supply) {
            SupplyPropertyTemplateComponent supplyPropertyTemplate = supply.supplyPropertyTemplate;
            long templateId = supplyPropertyTemplate.Template.TemplateId;
            Entity entity = CreateEntity(templateId, supplyPropertyTemplate.ConfigPath);
            supply.supplyGroup.Attach(entity);
        }

        public class SupplyNode : Node {
            public CooldownConfigComponent cooldownConfig;

            public SupplyGroupComponent supplyGroup;
            public SupplyPropertyTemplateComponent supplyPropertyTemplate;

            public SupplyTypeComponent supplyType;
        }
    }
}