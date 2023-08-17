using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class HealingPropertySystem : ECSSystem {
        [OnEventFire]
        public void UpdateValues(UpdateItemPropertiesEvent e, ItemNode item, [JoinByScreen] ItemUINode itemProperty) {
            itemProperty.propertyUI.SetValue(e.GetValue(item.healingProperty));
            itemProperty.propertyUI.SetNextValue(e.GetNextValue(item.healingProperty));
        }

        [OnEventFire]
        public void ShowProperty(ShowItemPropertiesEvent e, ItemNode item,
            [JoinByScreen] SingleNode<ItemPropertiesScreenComponent> screen) =>
            screen.component.GetComponentsInChildren<HealingGarageItemPropertyComponent>(true)[0].gameObject.SetActive(true);

        public class ItemNode : Node {
            public HealingPropertyComponent healingProperty;
        }

        public class ItemUINode : Node {
            public HealingGarageItemPropertyComponent healingGarageItemProperty;

            public PropertyUIComponent propertyUI;
        }
    }
}