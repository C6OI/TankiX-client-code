using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class AccelerationPropertySystem : ECSSystem {
        [OnEventFire]
        public void UpdateValues(UpdateItemPropertiesEvent e, ItemNode item, [JoinByScreen] ItemUINode itemProperty) {
            itemProperty.propertyUI.SetValue(e.GetValue(item.accelerationProperty));
            itemProperty.propertyUI.SetNextValue(e.GetNextValue(item.accelerationProperty));
        }

        [OnEventFire]
        public void ShowProperty(ShowItemPropertiesEvent e, ItemNode item,
            [JoinByScreen] SingleNode<ItemPropertiesScreenComponent> screen) =>
            screen.component.GetComponentsInChildren<AccelerationGarageItemPropertyComponent>(true)[0].gameObject
                .SetActive(true);

        public class ItemNode : Node {
            public AccelerationPropertyComponent accelerationProperty;
        }

        public class ItemUINode : Node {
            public AccelerationGarageItemPropertyComponent accelerationGarageItemProperty;

            public PropertyUIComponent propertyUI;
        }
    }
}