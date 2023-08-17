using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class TemperatureHittingTimePropertySystem : ECSSystem {
        [OnEventFire]
        public void UpdateValues(UpdateItemPropertiesEvent e, ItemNode item, [JoinByScreen] ItemUINode itemProperty) {
            itemProperty.propertyUI.SetValue(e.GetValue(item.temperatureHittingTimeProperty));
            itemProperty.propertyUI.SetNextValue(e.GetNextValue(item.temperatureHittingTimeProperty));
        }

        [OnEventFire]
        public void ShowProperty(ShowItemPropertiesEvent e, ItemNode item,
            [JoinByScreen] SingleNode<ItemPropertiesScreenComponent> screen) =>
            screen.component.GetComponentsInChildren<TemperatureHittingTimeGarageItemPropertyComponent>(true)[0].gameObject
                .SetActive(true);

        public class ItemNode : Node {
            public TemperatureHittingTimePropertyComponent temperatureHittingTimeProperty;
        }

        public class ItemUINode : Node {
            public PropertyUIComponent propertyUI;
            public TemperatureHittingTimeGarageItemPropertyComponent temperatureHittingTimeGarageItemProperty;
        }
    }
}