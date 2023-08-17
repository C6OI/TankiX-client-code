using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class SpinUpTimePropertySystem : ECSSystem {
        [OnEventFire]
        public void UpdateValues(UpdateItemPropertiesEvent e, ItemNode item, [JoinByScreen] ItemUINode itemProperty) {
            itemProperty.propertyUI.SetValue(e.GetValue(item.spinUpTimeProperty));
            itemProperty.propertyUI.SetNextValue(e.GetNextValue(item.spinUpTimeProperty));
        }

        [OnEventFire]
        public void ShowProperty(ShowItemPropertiesEvent e, ItemNode item,
            [JoinByScreen] SingleNode<ItemPropertiesScreenComponent> screen) =>
            screen.component.GetComponentsInChildren<SpinUpTimeGarageItemPropertyComponent>(true)[0].gameObject
                .SetActive(true);

        public class ItemNode : Node {
            public SpinUpTimePropertyComponent spinUpTimeProperty;
        }

        public class ItemUINode : Node {
            public PropertyUIComponent propertyUI;
            public SpinUpTimeGarageItemPropertyComponent spinUpTimeGarageItemProperty;
        }
    }
}