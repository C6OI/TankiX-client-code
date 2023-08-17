using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class ReloadMagazineTimePropertySystem : ECSSystem {
        [OnEventFire]
        public void UpdateValues(UpdateItemPropertiesEvent e, ItemNode item, [JoinByScreen] ItemUINode itemProperty) {
            itemProperty.propertyUI.SetValue(e.GetValue(item.reloadMagazineTimeProperty));
            itemProperty.propertyUI.SetNextValue(e.GetNextValue(item.reloadMagazineTimeProperty));
        }

        [OnEventFire]
        public void ShowProperty(ShowItemPropertiesEvent e, ItemNode item,
            [JoinByScreen] SingleNode<ItemPropertiesScreenComponent> screen) =>
            screen.component.GetComponentsInChildren<ReloadMagazineTimeGarageItemPropertyComponent>(true)[0].gameObject
                .SetActive(true);

        public class ItemNode : Node {
            public ReloadMagazineTimePropertyComponent reloadMagazineTimeProperty;
        }

        public class ItemUINode : Node {
            public PropertyUIComponent propertyUI;
            public ReloadMagazineTimeGarageItemPropertyComponent reloadMagazineTimeGarageItemProperty;
        }
    }
}