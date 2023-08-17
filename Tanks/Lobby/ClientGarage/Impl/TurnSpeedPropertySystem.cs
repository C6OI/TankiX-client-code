using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class TurnSpeedPropertySystem : ECSSystem {
        [OnEventFire]
        public void UpdateValues(UpdateItemPropertiesEvent e, ItemNode item, [JoinByScreen] ItemUINode itemProperty) {
            itemProperty.propertyUI.SetValue(e.GetValue(item.turnSpeedProperty));
            itemProperty.propertyUI.SetNextValue(e.GetNextValue(item.turnSpeedProperty));
        }

        [OnEventFire]
        public void ShowProperty(ShowItemPropertiesEvent e, ItemNode item,
            [JoinByScreen] SingleNode<ItemPropertiesScreenComponent> screen) =>
            screen.component.GetComponentsInChildren<TurnSpeedGarageItemPropertyComponent>(true)[0].gameObject
                .SetActive(true);

        public class ItemNode : Node {
            public TurnSpeedPropertyComponent turnSpeedProperty;
        }

        public class ItemUINode : Node {
            public PropertyUIComponent propertyUI;
            public TurnSpeedGarageItemPropertyComponent turnSpeedGarageItemProperty;
        }
    }
}