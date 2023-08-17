using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class FireDamagePropertySystem : ECSSystem {
        [OnEventFire]
        public void UpdateValues(UpdateItemPropertiesEvent e, ItemNode item, [JoinByScreen] ItemUINode itemProperty,
            [JoinAll] SingleNode<TemperatureConfigComponent> temperatureConfig) {
            TemperatureLimitPropertyComponent temperatureLimitProperty = item.temperatureLimitProperty;
            float maxFlameDamage = temperatureConfig.component.MaxFlameDamage;
            float minFlameDamage = temperatureConfig.component.MinFlameDamage;
            itemProperty.propertyUI.SetValue(minFlameDamage, e.GetValue(temperatureLimitProperty) * maxFlameDamage);
            itemProperty.propertyUI.SetNextValue(minFlameDamage, e.GetNextValue(temperatureLimitProperty) * maxFlameDamage);
        }

        [OnEventFire]
        public void ShowProperty(ShowItemPropertiesEvent e, ItemNode item,
            [JoinByScreen] SingleNode<ItemPropertiesScreenComponent> screen) =>
            screen.component.GetComponentsInChildren<FireDamageGarageItemPropertyComponent>(true)[0].gameObject
                .SetActive(true);

        public class ItemNode : Node {
            public TemperatureLimitPropertyComponent temperatureLimitProperty;
        }

        public class ItemUINode : Node {
            public FireDamageGarageItemPropertyComponent fireDamageGarageItemProperty;

            public PropertyUIComponent propertyUI;
        }
    }
}