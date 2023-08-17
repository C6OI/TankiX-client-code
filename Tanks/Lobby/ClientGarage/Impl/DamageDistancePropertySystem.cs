using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class DamageDistancePropertySystem : ECSSystem {
        [OnEventFire]
        public void UpdateValues(UpdateItemPropertiesEvent e, ItemNode item, [JoinByScreen] ItemUINode itemProperty) {
            itemProperty.propertyUI.SetValue(e.GetValue(item.damageDistanceProperty));
            itemProperty.propertyUI.SetNextValue(e.GetNextValue(item.damageDistanceProperty));
        }

        [OnEventFire]
        public void ShowProperty(ShowItemPropertiesEvent e, ItemNode item,
            [JoinByScreen] SingleNode<ItemPropertiesScreenComponent> screen) =>
            screen.component.GetComponentsInChildren<DamageDistanceGarageItemPropertyComponent>(true)[0].gameObject
                .SetActive(true);

        public class ItemNode : Node {
            public DamageDistancePropertyComponent damageDistanceProperty;
        }

        public class ItemUINode : Node {
            public DamageDistanceGarageItemPropertyComponent damageDistanceGarageItemProperty;

            public PropertyUIComponent propertyUI;
        }
    }
}