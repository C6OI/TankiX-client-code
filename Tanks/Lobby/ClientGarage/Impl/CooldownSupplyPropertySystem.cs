using System.Collections.Generic;
using System.Linq;
using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientGarage.API;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class CooldownSupplyPropertySystem : ECSSystem {
        [OnEventFire]
        public void ShowProperty(ShowItemPropertiesEvent e, CooldownPropertyNode item,
            [JoinByScreen] SingleNode<ItemPropertiesScreenComponent> screen) {
            CooldownConfigComponent cooldownConfig = item.cooldownConfig;

            screen.component.ShowPropertyAndSetValue<SelfCooldownSupplyPropertyComponent>(
                ToSeconds(cooldownConfig.SelfCooldown));

            Dictionary<SupplyType, GameObject> typeToGameObject = screen.component
                .GetComponentsInChildren<DependentCooldownSupplyPropertyComponent>(true)
                .ToDictionary(c => c.supplyType, c => c.gameObject);

            cooldownConfig.Dependencies.ForEach(delegate(ConfigDependentCooldown cooldown) {
                GameObject gameObject = typeToGameObject[cooldown.Dependency];
                gameObject.SetActive(true);
                gameObject.GetComponent<PropertyUIComponent>().SetValue(ToSeconds(cooldown.Cooldown));
                gameObject.GetComponent<PropertyUIComponent>().SetNextValue(ToSeconds(cooldown.Cooldown));
            });
        }

        [OnEventComplete]
        public void ShowRepairCooldownIcon(ShowItemPropertiesEvent e, CooldownPropertyNode item,
            [JoinByScreen] SelfCooldownPropertyNode selfCooldown) {
            selfCooldown.selfCooldownIconSupplyProperty.ArmorIcon.SetActive(false);
            selfCooldown.selfCooldownIconSupplyProperty.RepairIcon.SetActive(false);
            selfCooldown.selfCooldownIconSupplyProperty.DamageIcon.SetActive(false);
            selfCooldown.selfCooldownIconSupplyProperty.MineIcon.SetActive(false);
            selfCooldown.selfCooldownIconSupplyProperty.SpeedIcon.SetActive(false);

            switch (item.supplyType.Type) {
                case SupplyType.ARMOR:
                    selfCooldown.selfCooldownIconSupplyProperty.ArmorIcon.SetActive(true);
                    break;

                case SupplyType.REPAIR:
                    selfCooldown.selfCooldownIconSupplyProperty.RepairIcon.SetActive(true);
                    break;

                case SupplyType.DAMAGE:
                    selfCooldown.selfCooldownIconSupplyProperty.DamageIcon.SetActive(true);
                    break;

                case SupplyType.MINE:
                    selfCooldown.selfCooldownIconSupplyProperty.MineIcon.SetActive(true);
                    break;

                case SupplyType.SPEED:
                    selfCooldown.selfCooldownIconSupplyProperty.SpeedIcon.SetActive(true);
                    break;
            }
        }

        static int ToSeconds(int timeInMs) => timeInMs / 1000;

        public class CooldownPropertyNode : Node {
            public CooldownConfigComponent cooldownConfig;

            public ScreenGroupComponent screenGroup;
            public SupplyCountComponent supplyCount;

            public SupplyTypeComponent supplyType;
        }

        public class SelfCooldownPropertyNode : Node {
            public SelfCooldownIconSupplyPropertyComponent selfCooldownIconSupplyProperty;
            public SelfCooldownSupplyPropertyComponent selfCooldownSupplyProperty;
        }
    }
}