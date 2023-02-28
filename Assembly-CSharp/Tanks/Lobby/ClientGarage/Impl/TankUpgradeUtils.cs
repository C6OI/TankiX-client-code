using System.Collections.Generic;
using Tanks.Lobby.ClientGarage.API;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class TankUpgradeUtils {
        public static float CalculateUpgradeCoeff(List<int[]> modulesParams, int slotCount, ModuleUpgradablePowerConfigComponent moduleUpgradablePowerConfig) {
            int num = CalculateMaximumPercentSum(moduleUpgradablePowerConfig, slotCount);

            if (num < 0) {
                return -1f;
            }

            int num2 = CollectPercentSum(modulesParams, moduleUpgradablePowerConfig);
            return num2 / (float)num;
        }

        static int CalculateMaximumPercentSum(ModuleUpgradablePowerConfigComponent moduleUpgradablePowerConfig, int slotCount) {
            int index = moduleUpgradablePowerConfig.Level2PowerByTier.Count - 1;
            List<int> list = moduleUpgradablePowerConfig.Level2PowerByTier[index];
            int num = list[list.Count - 1];
            return num * slotCount;
        }

        static int CollectPercentSum(List<int[]> modulesParams, ModuleUpgradablePowerConfigComponent moduleUpgradablePowerConfig) {
            int num = 0;
            List<List<int>> level2PowerByTier = moduleUpgradablePowerConfig.Level2PowerByTier;

            foreach (int[] modulesParam in modulesParams) {
                int index = modulesParam[0];
                int index2 = Mathf.Min(modulesParam[1], level2PowerByTier[index].Count - 1);
                num += level2PowerByTier[index][index2];
            }

            return num;
        }
    }
}