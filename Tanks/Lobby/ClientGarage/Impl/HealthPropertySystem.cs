using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class HealthPropertySystem : ECSSystem {
        [OnEventFire]
        public void UpdateValues(UpdateItemPropertiesEvent e, ItemNode item, [JoinByScreen] ItemUINode itemProperty) {
            itemProperty.propertyUI.SetValue(e.GetValue(item.healthProperty));
            itemProperty.propertyUI.SetNextValue(e.GetNextValue(item.healthProperty));
        }

        [OnEventFire]
        public void ShowProperty(ShowItemPropertiesEvent e, ItemNode item,
            [JoinByScreen] SingleNode<ItemPropertiesScreenComponent> screen) =>
            screen.component.GetComponentsInChildren<HealthGarageItemPropertyComponent>(true)[0].gameObject.SetActive(true);

        public class ItemNode : Node {
            public HealthPropertyComponent healthProperty;
        }

        public class ItemUINode : Node {
            public HealthGarageItemPropertyComponent healthGarageItemProperty;

            public PropertyUIComponent propertyUI;
        }
    }
}