using System.Collections.Generic;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class ModuleTooltipData {
        public string desc;

        public int maxLevel;
        public string name;

        public List<ModuleVisualProperty> properties;

        public int upgradeLevel;

        public ModuleTooltipData(string name, string desc, int upgradeLevel, int maxLevel, List<ModuleVisualProperty> properties) {
            this.name = name;
            this.desc = desc;
            this.upgradeLevel = upgradeLevel;
            this.maxLevel = maxLevel;
            this.properties = properties;
        }
    }
}