using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class PressEnergyContextBuyButtonEvent : Event {
        public PressEnergyContextBuyButtonEvent() { }

        public PressEnergyContextBuyButtonEvent(long count, long price) {
            Count = count;
            XPrice = price;
        }

        public long XPrice { get; set; }

        public long Count { get; set; }
    }
}