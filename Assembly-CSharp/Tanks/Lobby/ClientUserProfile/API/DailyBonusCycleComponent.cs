using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientUserProfile.API {
    public class DailyBonusCycleComponent : Component {
        public DailyBonusData[] DailyBonuses { get; set; }

        public int[] Zones { get; set; }
    }
}