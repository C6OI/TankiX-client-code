using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class HammerDamagePropertySystem : ECSSystem {
        [OnEventFire]
        public void UpdateValues(UpdateItemPropertiesEvent e, ItemNode item, [JoinByScreen] ItemUINode itemProperty) {
            DamagePerPelletPropertyComponent damagePerPelletProperty = item.damagePerPelletProperty;
            MagazineSizePropertyComponent magazineSizeProperty = item.magazineSizeProperty;
            itemProperty.propertyUI.SetValue(e.GetValue(damagePerPelletProperty) * e.GetValue(magazineSizeProperty));
            itemProperty.propertyUI.SetNextValue(e.GetNextValue(damagePerPelletProperty) * e.GetValue(magazineSizeProperty));
        }

        [OnEventFire]
        public void ShowProperty(ShowItemPropertiesEvent e, ItemNode item,
            [JoinByScreen] SingleNode<ItemPropertiesScreenComponent> screen) =>
            screen.component.GetComponentsInChildren<DamageGarageItemPropertyComponent>(true)[0].gameObject.SetActive(true);

        public class ItemNode : Node {
            public DamagePerPelletPropertyComponent damagePerPelletProperty;

            public MagazineSizePropertyComponent magazineSizeProperty;
        }

        public class ItemUINode : Node {
            public DamageGarageItemPropertyComponent damageGarageItemProperty;

            public PropertyUIComponent propertyUI;
        }
    }
}