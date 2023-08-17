using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class TurretTurnSpeedPropertySystem : ECSSystem {
        [OnEventFire]
        public void UpdateValues(UpdateItemPropertiesEvent e, ItemNode item, [JoinByScreen] ItemUINode itemProperty) {
            itemProperty.propertyUI.SetValue(e.GetValue(item.turretTurnSpeedProperty));
            itemProperty.propertyUI.SetNextValue(e.GetNextValue(item.turretTurnSpeedProperty));
        }

        [OnEventFire]
        public void ShowProperty(ShowItemPropertiesEvent e, ItemNode item,
            [JoinByScreen] SingleNode<ItemPropertiesScreenComponent> screen) =>
            screen.component.GetComponentsInChildren<TurretTurnSpeedGarageItemPropertyComponent>(true)[0].gameObject
                .SetActive(true);

        public class ItemNode : Node {
            public TurretTurnSpeedPropertyComponent turretTurnSpeedProperty;
        }

        public class ItemUINode : Node {
            public PropertyUIComponent propertyUI;
            public TurretTurnSpeedGarageItemPropertyComponent turretTurnSpeedGarageItemProperty;
        }
    }
}