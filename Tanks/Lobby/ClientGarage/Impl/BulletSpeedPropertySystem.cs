using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class BulletSpeedPropertySystem : ECSSystem {
        [OnEventFire]
        public void UpdateValues(UpdateItemPropertiesEvent e, ItemNode item, [JoinByScreen] ItemUINode itemProperty) {
            itemProperty.propertyUI.SetValue(e.GetValue(item.bulletSpeedProperty));
            itemProperty.propertyUI.SetNextValue(e.GetNextValue(item.bulletSpeedProperty));
        }

        [OnEventFire]
        public void ShowProperty(ShowItemPropertiesEvent e, ItemNode item,
            [JoinByScreen] SingleNode<ItemPropertiesScreenComponent> screen) =>
            screen.component.GetComponentsInChildren<BulletSpeedGarageItemPropertyComponent>(true)[0].gameObject
                .SetActive(true);

        public class ItemNode : Node {
            public BulletSpeedPropertyComponent bulletSpeedProperty;
        }

        public class ItemUINode : Node {
            public BulletSpeedGarageItemPropertyComponent bulletSpeedGarageItemProperty;

            public PropertyUIComponent propertyUI;
        }
    }
}