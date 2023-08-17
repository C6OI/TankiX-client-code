using System;
using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class RicochetShotsCountPropertySystem : ECSSystem {
        [OnEventFire]
        public void UpdateValues(UpdateItemPropertiesEvent e, ItemNode item, [JoinByScreen] ItemUINode itemProperty) {
            itemProperty.propertyUI.SetValue(GetShotsNumber(item, e.GetValue));
            itemProperty.propertyUI.SetNextValue(GetShotsNumber(item, e.GetNextValue));
        }

        static float GetShotsNumber(ItemNode item, Func<UpgradablePropertyComponent, float> getValue) => GetShotsNumber(
            getValue(item.reloadTimeProperty),
            getValue(item.energyChargePerShotProperty),
            getValue(item.energyRechargeSpeedProperty));

        static float GetShotsNumber(float reload, float perShot, float charge) =>
            Mathf.Floor((1f - perShot) / (perShot - charge * reload)) + 1f;

        [OnEventFire]
        public void ShowProperty(ShowItemPropertiesEvent e, ItemNode item,
            [JoinByScreen] SingleNode<ItemPropertiesScreenComponent> screen) =>
            screen.component.GetComponentsInChildren<MagazineSizeGarageItemPropertyComponent>(true)[0].gameObject
                .SetActive(true);

        [Not(typeof(AimingMinDamagePropertyComponent))]
        public class ItemNode : Node {
            public EnergyChargePerShotPropertyComponent energyChargePerShotProperty;
            public EnergyRechargeSpeedPropertyComponent energyRechargeSpeedProperty;

            public ReloadTimePropertyComponent reloadTimeProperty;
        }

        public class ItemUINode : Node {
            public MagazineSizeGarageItemPropertyComponent magazineSizeGarageItemProperty;

            public PropertyUIComponent propertyUI;
        }
    }
}