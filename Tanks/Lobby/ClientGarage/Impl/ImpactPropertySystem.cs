using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class ImpactPropertySystem : ECSSystem {
        [OnEventFire]
        public void UpdateValues(UpdateItemPropertiesEvent e, ItemNode item, [JoinByScreen] ItemUINode itemProperty) {
            itemProperty.propertyUI.SetValue(e.GetValue(item.impactProperty));
            itemProperty.propertyUI.SetNextValue(e.GetNextValue(item.impactProperty));
        }

        [OnEventFire]
        public void ShowProperty(ShowItemPropertiesEvent e, ItemNode item,
            [JoinByScreen] SingleNode<ItemPropertiesScreenComponent> screen) =>
            screen.component.GetComponentsInChildren<ImpactGarageItemPropertyComponent>(true)[0].gameObject.SetActive(true);

        public class ItemNode : Node {
            public ImpactPropertyComponent impactProperty;
        }

        public class ItemUINode : Node {
            public ImpactGarageItemPropertyComponent impactGarageItemProperty;

            public PropertyUIComponent propertyUI;
        }
    }
}