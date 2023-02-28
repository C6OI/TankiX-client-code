using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientUserProfile.API {
    public class DailyBonusData {
        DailyBonusType dailyBonusType;

        public long Code { get; set; }

        public DailyBonusType DailyBonusType {
            get {
                if (dailyBonusType != 0) {
                    return dailyBonusType;
                }

                if (ContainerReward != null) {
                    return DailyBonusType.CONTAINER;
                }

                if (DetailReward != null) {
                    return DailyBonusType.DETAIL;
                }

                if (CryAmount > 0) {
                    return DailyBonusType.CRY;
                }

                if (XcryAmount > 0) {
                    return DailyBonusType.XCRY;
                }

                if (EnergyAmount > 0) {
                    return DailyBonusType.ENERGY;
                }

                return DailyBonusType.NONE;
            }
        }

        [ProtocolOptional] public long CryAmount { get; set; }

        [ProtocolOptional] public long XcryAmount { get; set; }

        [ProtocolOptional] public long EnergyAmount { get; set; }

        [ProtocolOptional] public DailyBonusGarageItemReward ContainerReward { get; set; }

        [ProtocolOptional] public DailyBonusGarageItemReward DetailReward { get; set; }

        public bool IsEpic() => DailyBonusType == DailyBonusType.CONTAINER || DailyBonusType == DailyBonusType.DETAIL || DailyBonusType == DailyBonusType.XCRY ||
                                DailyBonusType == DailyBonusType.ENERGY;
    }
}