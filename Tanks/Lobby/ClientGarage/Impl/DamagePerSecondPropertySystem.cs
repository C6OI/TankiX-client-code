using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class DamagePerSecondPropertySystem : ECSSystem {
        [OnEventFire]
        public void UpdateValues(UpdateItemPropertiesEvent e, ItemNode item, [JoinByScreen] ItemUINode itemProperty) {
            itemProperty.propertyUI.SetValue(e.GetValue(item.damagePerSecondProperty));
            itemProperty.propertyUI.SetNextValue(e.GetNextValue(item.damagePerSecondProperty));
        }

        [OnEventFire]
        public void ShowProperty(ShowItemPropertiesEvent e, ItemNode item,
            [JoinByScreen] SingleNode<ItemPropertiesScreenComponent> screen) =>
            screen.component.GetComponentsInChildren<DamageGarageItemPropertyComponent>(true)[0].gameObject.SetActive(true);

        public class ItemNode : Node {
            public DamagePerSecondPropertyComponent damagePerSecondProperty;
        }

        public class ItemUINode : Node {
            public DamageGarageItemPropertyComponent damageGarageItemProperty;

            public PropertyUIComponent propertyUI;
        }
    }
}