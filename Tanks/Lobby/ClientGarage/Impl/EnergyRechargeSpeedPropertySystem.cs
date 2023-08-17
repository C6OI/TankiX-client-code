using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class EnergyRechargeSpeedPropertySystem : ECSSystem {
        [OnEventFire]
        public void UpdateValues(UpdateItemPropertiesEvent e, ItemNode item, [JoinByScreen] ItemUINode itemProperty) {
            itemProperty.propertyUI.SetValue(1f / e.GetValue(item.energyRechargeSpeedProperty));
            itemProperty.propertyUI.SetNextValue(1f / e.GetNextValue(item.energyRechargeSpeedProperty));
        }

        [OnEventFire]
        public void ShowProperty(ShowItemPropertiesEvent e, ItemNode item,
            [JoinByScreen] SingleNode<ItemPropertiesScreenComponent> screen) =>
            screen.component.GetComponentsInChildren<EnergyRechargeSpeedGarageItemPropertyComponent>(true)[0].gameObject
                .SetActive(true);

        public class ItemNode : Node {
            public EnergyRechargeSpeedPropertyComponent energyRechargeSpeedProperty;
        }

        public class ItemUINode : Node {
            public EnergyRechargeSpeedGarageItemPropertyComponent energyRechargeSpeedGarageItemProperty;

            public PropertyUIComponent propertyUI;
        }
    }
}