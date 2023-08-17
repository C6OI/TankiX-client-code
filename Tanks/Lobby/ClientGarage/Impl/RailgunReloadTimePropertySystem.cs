using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class RailgunReloadTimePropertySystem : ECSSystem {
        [OnEventFire]
        public void UpdateValues(UpdateItemPropertiesEvent e, ItemNode item, [JoinByScreen] ItemUINode itemProperty) {
            ReloadTimePropertyComponent reloadTimeProperty = item.reloadTimeProperty;
            ChargeTimePropertyComponent chargeTimeProperty = item.chargeTimeProperty;
            itemProperty.propertyUI.SetValue(e.GetValue(reloadTimeProperty) + e.GetValue(chargeTimeProperty));
            itemProperty.propertyUI.SetNextValue(e.GetNextValue(reloadTimeProperty) + e.GetNextValue(chargeTimeProperty));
        }

        [OnEventFire]
        public void ShowProperty(ShowItemPropertiesEvent e, ItemNode item,
            [JoinByScreen] SingleNode<ItemPropertiesScreenComponent> screen) =>
            screen.component.GetComponentsInChildren<ReloadTimeGarageItemPropertyComponent>(true)[0].gameObject
                .SetActive(true);

        public class ItemNode : Node {
            public ChargeTimePropertyComponent chargeTimeProperty;
            public ReloadTimePropertyComponent reloadTimeProperty;
        }

        public class ItemUINode : Node {
            public PropertyUIComponent propertyUI;
            public ReloadTimeGarageItemPropertyComponent reloadTimeGarageItemProperty;
        }
    }
}