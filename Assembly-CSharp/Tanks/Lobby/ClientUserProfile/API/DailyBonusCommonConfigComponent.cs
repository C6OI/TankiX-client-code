using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientUserProfile.API {
    [SerialVersionUID(636462569803130386L)]
    public class DailyBonusCommonConfigComponent : Component, AttachToEntityListener {
        public static Entity DailyBonusConfig;

        public long ReceivingBonusIntervalInSeconds { get; set; }

        public long BattleCountToUnlockDailyBonuses { get; set; }

        public int PremiumTimeSpeedUp { get; set; }

        public void AttachedToEntity(Entity entity) {
            DailyBonusConfig = entity;
        }
    }
}