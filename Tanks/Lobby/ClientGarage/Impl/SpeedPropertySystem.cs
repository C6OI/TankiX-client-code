using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class SpeedPropertySystem : ECSSystem {
        [OnEventFire]
        public void UpdateValues(UpdateItemPropertiesEvent e, ItemNode item, [JoinByScreen] ItemUINode itemProperty) {
            itemProperty.propertyUI.SetValue(e.GetValue(item.speedProperty));
            itemProperty.propertyUI.SetNextValue(e.GetNextValue(item.speedProperty));
        }

        [OnEventFire]
        public void ShowProperty(ShowItemPropertiesEvent e, ItemNode item,
            [JoinByScreen] SingleNode<ItemPropertiesScreenComponent> screen) =>
            screen.component.GetComponentsInChildren<SpeedGarageItemPropertyComponent>(true)[0].gameObject.SetActive(true);

        public class ItemNode : Node {
            public SpeedPropertyComponent speedProperty;
        }

        public class ItemUINode : Node {
            public PropertyUIComponent propertyUI;
            public SpeedGarageItemPropertyComponent speedGarageItemProperty;
        }
    }
}