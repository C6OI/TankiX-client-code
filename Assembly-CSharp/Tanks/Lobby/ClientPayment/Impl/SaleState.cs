namespace Tanks.Lobby.ClientPayment.Impl {
    public class SaleState {
        public SaleState() {
            Reset();
        }

        public double PriceMultiplier { get; set; }

        public double AmountMultiplier { get; set; }

        public void Reset() {
            PriceMultiplier = 1.0;
            AmountMultiplier = 1.0;
        }
    }
}