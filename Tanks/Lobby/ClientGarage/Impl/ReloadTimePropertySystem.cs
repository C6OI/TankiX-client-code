using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class ReloadTimePropertySystem : ECSSystem {
        [OnEventFire]
        public void UpdateValues(UpdateItemPropertiesEvent e, ItemNode item, [JoinByScreen] ItemUINode itemProperty) {
            itemProperty.propertyUI.SetValue(e.GetValue(item.reloadTimeProperty));
            itemProperty.propertyUI.SetNextValue(e.GetNextValue(item.reloadTimeProperty));
        }

        [OnEventFire]
        public void ShowProperty(ShowItemPropertiesEvent e, ItemNode item,
            [JoinByScreen] SingleNode<ItemPropertiesScreenComponent> screen) =>
            screen.component.GetComponentsInChildren<ReloadTimeGarageItemPropertyComponent>(true)[0].gameObject
                .SetActive(true);

        [Not(typeof(ChargeTimePropertyComponent))]
        public class ItemNode : Node {
            public ReloadTimePropertyComponent reloadTimeProperty;
        }

        public class ItemUINode : Node {
            public PropertyUIComponent propertyUI;
            public ReloadTimeGarageItemPropertyComponent reloadTimeGarageItemProperty;
        }
    }
}