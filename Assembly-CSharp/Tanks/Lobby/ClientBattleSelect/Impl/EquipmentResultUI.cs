using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientGarage.Impl;
using Tanks.Lobby.ClientProfile.API;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class EquipmentResultUI : ProgressResultUI {
        [SerializeField] new TextMeshProUGUI name;

        [Inject] public static GarageItemsRegistry GarageItemsRegistry { get; set; }

        public void SetProgress(Entity itemEntity, float expReward, int previousUpgradeLevel, int[] levels, BattleResultsTextTemplatesComponent textTemplates) {
            TankPartItem item = GarageItemsRegistry.GetItem<TankPartItem>(itemEntity.GetComponent<MarketItemGroupComponent>().Key);
            name.text = item.Name;
            bool flag = previousUpgradeLevel == UpgradablePropertiesUtils.MAX_LEVEL;
            bool flag2 = item.UpgradeLevel == UpgradablePropertiesUtils.MAX_LEVEL;
            long absExp = !flag ? !flag2 ? item.AbsExperience : levels[levels.Length - 1] : levels[levels.Length - 1] + (long)expReward;
            LevelInfo levelInfo = LevelInfo.Get(absExp, levels);
            SetProgress(expReward, levels, levelInfo, textTemplates);
        }

        public void SetNewLevel() {
            SetResidualProgress();
        }
    }
}