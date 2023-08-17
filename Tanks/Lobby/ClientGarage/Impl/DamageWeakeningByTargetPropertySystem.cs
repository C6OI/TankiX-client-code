using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class DamageWeakeningByTargetPropertySystem : ECSSystem {
        [OnEventFire]
        public void UpdateValues(UpdateItemPropertiesEvent e, ItemNode item, [JoinByScreen] ItemUINode itemProperty) {
            itemProperty.propertyUI.SetValue(e.GetValue(item.damageWeakeningByTargetProperty));
            itemProperty.propertyUI.SetNextValue(e.GetNextValue(item.damageWeakeningByTargetProperty));
        }

        [OnEventFire]
        public void ShowProperty(ShowItemPropertiesEvent e, ItemNode item,
            [JoinByScreen] SingleNode<ItemPropertiesScreenComponent> screen) =>
            screen.component.GetComponentsInChildren<DamageWeakeningByTargetGarageItemPropertyComponent>(true)[0].gameObject
                .SetActive(true);

        public class ItemNode : Node {
            public DamageWeakeningByTargetPropertyComponent damageWeakeningByTargetProperty;
        }

        public class ItemUINode : Node {
            public DamageWeakeningByTargetGarageItemPropertyComponent damageWeakeningByTargetGarageItemProperty;

            public PropertyUIComponent propertyUI;
        }
    }
}