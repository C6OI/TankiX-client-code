using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class RangeDamagePropertySystem : ECSSystem {
        [OnEventFire]
        public void UpdateValues(UpdateItemPropertiesEvent e, ItemNode item, [JoinByScreen] ItemUINode itemProperty) {
            MinDamagePropertyComponent minDamageProperty = item.minDamageProperty;
            MaxDamagePropertyComponent maxDamageProperty = item.maxDamageProperty;
            itemProperty.propertyUI.SetValue(e.GetValue(minDamageProperty), e.GetValue(maxDamageProperty));
            itemProperty.propertyUI.SetNextValue(e.GetNextValue(minDamageProperty), e.GetNextValue(maxDamageProperty));
        }

        [OnEventFire]
        public void ShowProperty(ShowItemPropertiesEvent e, ItemNode item,
            [JoinByScreen] SingleNode<ItemPropertiesScreenComponent> screen) =>
            screen.component.GetComponentsInChildren<DamageGarageItemPropertyComponent>(true)[0].gameObject.SetActive(true);

        public class ItemNode : Node {
            public MaxDamagePropertyComponent maxDamageProperty;

            public MinDamagePropertyComponent minDamageProperty;
        }

        public class ItemUINode : Node {
            public DamageGarageItemPropertyComponent damageGarageItemProperty;

            public PropertyUIComponent propertyUI;
        }
    }
}