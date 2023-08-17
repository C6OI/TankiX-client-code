using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class MagazineSizePropertySystem : ECSSystem {
        [OnEventFire]
        public void UpdateValues(UpdateItemPropertiesEvent e, ItemNode item, [JoinByScreen] ItemUINode itemProperty) {
            itemProperty.propertyUI.SetValue(e.GetValue(item.magazineSizeProperty));
            itemProperty.propertyUI.SetNextValue(e.GetNextValue(item.magazineSizeProperty));
        }

        [OnEventFire]
        public void ShowProperty(ShowItemPropertiesEvent e, ItemNode item,
            [JoinByScreen] SingleNode<ItemPropertiesScreenComponent> screen) =>
            screen.component.GetComponentsInChildren<MagazineSizeGarageItemPropertyComponent>(true)[0].gameObject
                .SetActive(true);

        public class ItemNode : Node {
            public MagazineSizePropertyComponent magazineSizeProperty;
        }

        public class ItemUINode : Node {
            public MagazineSizeGarageItemPropertyComponent magazineSizeGarageItemProperty;

            public PropertyUIComponent propertyUI;
        }
    }
}