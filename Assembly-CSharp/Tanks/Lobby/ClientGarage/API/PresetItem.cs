using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientGarage.Impl;

namespace Tanks.Lobby.ClientGarage.API {
    public class PresetItem {
        public long hullId;

        public string hullName;

        public bool isSelected;

        public int level;
        public string Name;

        public Entity presetEntity;

        public string turretName;

        public long weaponId;

        public PresetItem(string name, int level, string hullName, string turretName, long hullId, long weaponId, Entity presetEntity) {
            Name = name;
            this.level = level;
            this.presetEntity = presetEntity;
            this.hullName = hullName;
            this.turretName = turretName;
            this.hullId = hullId;
            this.weaponId = weaponId;
            isSelected = presetEntity.HasComponent<SelectedPresetComponent>();
        }
    }
}