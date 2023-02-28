using System.Collections.Generic;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class ModuleVisualProperty {
        public ModuleVisualProperty() {
            Name = string.Empty;
            UpgradeLevel2Values = new float[2];
            Unit = string.Empty;
            Format = string.Empty;
        }

        public string Name { get; set; }

        public bool LinearInterpolation { get; set; }

        public float[] UpgradeLevel2Values { get; set; }

        public Dictionary<long, EffectProperty> EquipmentProperties { get; set; }

        public string Unit { get; set; }

        [ProtocolOptional] public string Format { get; set; }

        public bool MaxAndMin { get; set; }

        public string[] MaxAndMinString { get; set; }

        public bool ProgressBar { get; set; }

        public string SpriteUID { get; set; }

        public bool Upgradable => UpgradeLevel2Values.Length > 0 && UpgradeLevel2Values[0] != UpgradeLevel2Values[UpgradeLevel2Values.Length - 1];

        public float CalculateModuleEffectPropertyValue(int moduleUpgradeLevel, int maxModuleUpgradeLevel) {
            int num = UpgradeLevel2Values.Length;

            if (moduleUpgradeLevel > maxModuleUpgradeLevel) {
                moduleUpgradeLevel = maxModuleUpgradeLevel;
            }

            if (LinearInterpolation) {
                float num2 = moduleUpgradeLevel / (float)maxModuleUpgradeLevel;
                float num3 = UpgradeLevel2Values[0];
                float num4 = UpgradeLevel2Values[num - 1];
                return num3 + (num4 - num3) * num2;
            }

            return UpgradeLevel2Values[moduleUpgradeLevel];
        }
    }
}