using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class SelfHealingPropertySystem : ECSSystem {
        [OnEventFire]
        public void UpdateValues(UpdateItemPropertiesEvent e, ItemNode item, [JoinByScreen] ItemUINode itemProperty) {
            itemProperty.propertyUI.SetValue(e.GetValue(item.selfHealingProperty));
            itemProperty.propertyUI.SetNextValue(e.GetNextValue(item.selfHealingProperty));
        }

        [OnEventFire]
        public void ShowProperty(ShowItemPropertiesEvent e, ItemNode item,
            [JoinByScreen] SingleNode<ItemPropertiesScreenComponent> screen) =>
            screen.component.GetComponentsInChildren<SelfHealingGarageItemPropertyComponent>(true)[0].gameObject
                .SetActive(true);

        public class ItemNode : Node {
            public SelfHealingPropertyComponent selfHealingProperty;
        }

        public class ItemUINode : Node {
            public PropertyUIComponent propertyUI;
            public SelfHealingGarageItemPropertyComponent selfHealingGarageItemProperty;
        }
    }
}