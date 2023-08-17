using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class AimingDamagePropertySystem : ECSSystem {
        [OnEventFire]
        public void UpdateValues(UpdateItemPropertiesEvent e, ItemNode item, [JoinByScreen] ItemUINode itemProperty) {
            AimingMinDamagePropertyComponent aimingMinDamageProperty = item.aimingMinDamageProperty;
            AimingMaxDamagePropertyComponent aimingMaxDamageProperty = item.aimingMaxDamageProperty;
            itemProperty.propertyUI.SetValue(e.GetValue(aimingMinDamageProperty), e.GetValue(aimingMaxDamageProperty));

            itemProperty.propertyUI.SetNextValue(e.GetNextValue(aimingMinDamageProperty),
                e.GetNextValue(aimingMaxDamageProperty));
        }

        [OnEventFire]
        public void ShowProperty(ShowItemPropertiesEvent e, ItemNode item,
            [JoinByScreen] SingleNode<ItemPropertiesScreenComponent> screen) =>
            screen.component.GetComponentsInChildren<AimingDamageGarageItemPropertyComponent>(true)[0].gameObject
                .SetActive(true);

        [OnEventFire]
        public void UpdateShaftDamageValues(UpdateItemPropertiesEvent e, ItemNode item,
            [JoinByScreen] ItemDamageUINode itemProperty) {
            AimingMinDamagePropertyComponent aimingMinDamageProperty = item.aimingMinDamageProperty;
            itemProperty.propertyUI.SetValue(e.GetValue(aimingMinDamageProperty));
            itemProperty.propertyUI.SetNextValue(e.GetNextValue(aimingMinDamageProperty));
        }

        [OnEventFire]
        public void ShowShaftDamageProperty(ShowItemPropertiesEvent e, ItemNode item,
            [JoinByScreen] SingleNode<ItemPropertiesScreenComponent> screen) =>
            screen.component.GetComponentsInChildren<ShaftDamageGarageItemPropertyComponent>(true)[0].gameObject
                .SetActive(true);

        public class ItemNode : Node {
            public AimingMaxDamagePropertyComponent aimingMaxDamageProperty;
            public AimingMinDamagePropertyComponent aimingMinDamageProperty;
        }

        public class ItemUINode : Node {
            public AimingDamageGarageItemPropertyComponent aimingDamageGarageItemProperty;

            public PropertyUIComponent propertyUI;
        }

        public class ItemDamageUINode : Node {
            public PropertyUIComponent propertyUI;
            public ShaftDamageGarageItemPropertyComponent shaftDamageGarageItemProperty;
        }
    }
}