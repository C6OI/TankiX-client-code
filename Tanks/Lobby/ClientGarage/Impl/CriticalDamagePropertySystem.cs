using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class CriticalDamagePropertySystem : ECSSystem {
        [OnEventFire]
        public void UpdateValues(UpdateItemPropertiesEvent e, ItemNode item, [JoinByScreen] ItemUINode itemProperty) {
            itemProperty.propertyUI.SetValue(e.GetValue(item.criticalDamageProperty));
            itemProperty.propertyUI.SetNextValue(e.GetNextValue(item.criticalDamageProperty));
        }

        [OnEventFire]
        public void ShowProperty(ShowItemPropertiesEvent e, ItemNode item,
            [JoinByScreen] SingleNode<ItemPropertiesScreenComponent> screen) =>
            screen.component.GetComponentsInChildren<CriticalDamageGarageItemPropertyComponent>(true)[0].gameObject
                .SetActive(true);

        public class ItemNode : Node {
            public CriticalDamagePropertyComponent criticalDamageProperty;
        }

        public class ItemUINode : Node {
            public CriticalDamageGarageItemPropertyComponent criticalDamageGarageItemProperty;

            public PropertyUIComponent propertyUI;
        }
    }
}