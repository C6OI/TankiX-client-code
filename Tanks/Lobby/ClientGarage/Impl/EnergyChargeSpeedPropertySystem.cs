using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class EnergyChargeSpeedPropertySystem : ECSSystem {
        [OnEventFire]
        public void UpdateValues(UpdateItemPropertiesEvent e, ItemNode item, [JoinByScreen] ItemUINode itemProperty) {
            itemProperty.propertyUI.SetValue(1f / e.GetValue(item.energyChargeSpeedProperty));
            itemProperty.propertyUI.SetNextValue(1f / e.GetNextValue(item.energyChargeSpeedProperty));
        }

        [OnEventFire]
        public void ShowProperty(ShowItemPropertiesEvent e, ItemNode item,
            [JoinByScreen] SingleNode<ItemPropertiesScreenComponent> screen) =>
            screen.component.GetComponentsInChildren<EnergyChargeSpeedGarageItemPropertyComponent>(true)[0].gameObject
                .SetActive(true);

        public class ItemNode : Node {
            public EnergyChargeSpeedPropertyComponent energyChargeSpeedProperty;
        }

        public class ItemUINode : Node {
            public EnergyChargeSpeedGarageItemPropertyComponent energyChargeSpeedGarageItemProperty;

            public PropertyUIComponent propertyUI;
        }
    }
}