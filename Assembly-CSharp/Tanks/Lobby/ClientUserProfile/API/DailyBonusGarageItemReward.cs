namespace Tanks.Lobby.ClientUserProfile.API {
    public class DailyBonusGarageItemReward {
        public DailyBonusGarageItemReward() { }

        public DailyBonusGarageItemReward(long marketItemId, long amount) {
            MarketItemId = marketItemId;
            Amount = amount;
        }

        public long MarketItemId { get; set; }

        public long Amount { get; set; }

        public static bool IsValid(DailyBonusGarageItemReward reward) {
            if (reward == null) {
                return false;
            }

            return reward.IsValid();
        }

        bool IsValid() => Amount > 0;

        public override string ToString() => string.Format("DailyBonusGarageItemReward: [MarketItemId = {0}; Amount = {1}]", MarketItemId, Amount);
    }
}